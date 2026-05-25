using BLL.DTOs;
using BLL.Interfaces;
using DAL.Interfaces;
using Entity;

namespace BLL.Services;

/// <summary>
/// Lógica de negocio para el proceso de ventas y facturación.
/// Orquesta FEFO, cálculo de descuentos, construcción de detalles,
/// actualización de stock y anulación de ventas con reversión de inventario.
/// Es el servicio más crítico del sistema: coordina múltiples repositorios
/// en una única operación atómica delegada al repositorio de ventas.
/// </summary>
public class VentaService : IVentaService
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IDetalleVentaRepository _detalleVentaRepository;
    private readonly ILoteRepository _loteRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IParametroService _parametroService;

    public VentaService(
        IVentaRepository ventaRepository,
        IDetalleVentaRepository detalleVentaRepository,
        ILoteRepository loteRepository,
        IProductoRepository productoRepository,
        IUsuarioRepository usuarioRepository,
        IClienteRepository clienteRepository,
        IParametroService parametroService)
    {
        _ventaRepository = ventaRepository;
        _detalleVentaRepository = detalleVentaRepository;
        _loteRepository = loteRepository;
        _productoRepository = productoRepository;
        _usuarioRepository = usuarioRepository;
        _clienteRepository = clienteRepository;
        _parametroService = parametroService;
    }

    /// <summary>
    /// Procesa una venta completa aplicando la metodología FEFO.
    /// Por cada ítem de la lista:
    ///   1. Selecciona el lote con fecha de vencimiento más próxima (FEFO).
    ///   2. Verifica que haya stock suficiente.
    ///   3. Calcula el descuento por proximidad de vencimiento si aplica.
    ///   4. Construye el detalle de venta con trazabilidad al lote exacto.
    /// Luego delega la persistencia atómica (cabecera + detalles + descuento stock)
    /// al procedimiento almacenado SP_CREAR_VENTA a través del repositorio.
    /// </summary>
    public ResultadoOperacion RealizarVenta(
        List<(int productoId, int cantidad)> items,
        int usuarioId,
        int? clienteId = null)
    {
        try
        {
            if (items is null || items.Count == 0)
                return ResultadoOperacion.Fallo("La venta debe contener al menos un producto.");

            if (usuarioId <= 0)
                return ResultadoOperacion.Fallo("El identificador del usuario cajero no es válido.");

            // Verificar que el usuario cajero exista
            var usuario = _usuarioRepository.ObtenerPorId(usuarioId);
            if (usuario is null)
                return ResultadoOperacion.Fallo(
                    $"No se encontró ningún usuario con el ID {usuarioId}.");

            // Verificar cliente si se asoció a la venta
            if (clienteId.HasValue)
            {
                var cliente = _clienteRepository.ObtenerPorId(clienteId.Value);
                if (cliente is null)
                    return ResultadoOperacion.Fallo(
                        $"No se encontró ningún cliente con el ID {clienteId.Value}.");
            }

            // Validar que no haya productos duplicados en la lista
            var productosDuplicados = items
                .GroupBy(i => i.productoId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (productosDuplicados.Count > 0)
                return ResultadoOperacion.Fallo(
                    $"La lista contiene productos duplicados (IDs: {string.Join(", ", productosDuplicados)}). " +
                    "Consolide las cantidades en un solo ítem por producto.");

            // Validar cantidades individuales
            foreach (var (productoId, cantidad) in items)
            {
                if (productoId <= 0)
                    return ResultadoOperacion.Fallo(
                        $"El identificador de producto '{productoId}' no es válido.");

                if (cantidad <= 0)
                    return ResultadoOperacion.Fallo(
                        $"La cantidad para el producto {productoId} debe ser mayor a cero.");
            }

            int diasVentana = _parametroService.ObtenerDiasVentanaCritica();
            decimal descuentoGeneral = _parametroService.ObtenerPorcentajeDescuentoVencimiento();
            var detalles = new List<DetalleVenta>();
            var resumenDetalles = new List<object>();

            // Fase 1: Resolver FEFO y construir detalles — validar todo antes de persistir
            foreach (var (productoId, cantidad) in items)
            {
                var producto = _productoRepository.ObtenerPorId(productoId);
                if (producto is null)
                    return ResultadoOperacion.Fallo(
                        $"No se encontró ningún producto con el ID {productoId}.");

                var lote = _loteRepository.SeleccionarLoteFEFO(productoId);
                if (lote is null)
                    return ResultadoOperacion.Fallo(
                        $"No hay lotes activos disponibles para '{producto.Nombre}'. " +
                        "Verifique el inventario.");

                if (lote.CantidadActual < cantidad)
                    return ResultadoOperacion.Fallo(
                        $"Stock insuficiente para '{producto.Nombre}'. " +
                        $"Disponible: {lote.CantidadActual} unidad(es), solicitado: {cantidad}.");

                // Calcular descuento por proximidad de vencimiento
                decimal porcentajeDescuento = 0m;
                bool enVentanaCritica = lote.EstaEnVentanaCritica(diasVentana);

                if (enVentanaCritica)
                {
                    porcentajeDescuento = producto.UsaDescuentoPersonalizado
                        ? producto.DescuentoProximidadVencimiento!.Value
                        : descuentoGeneral;
                }

                decimal descuentoUnitario = Math.Round(lote.PrecioVenta * porcentajeDescuento, 2);
                decimal precioAplicado = lote.PrecioVenta - descuentoUnitario;

                detalles.Add(new DetalleVenta
                {
                    LoteId = lote.Id,
                    Cantidad = cantidad,
                    PrecioAplicado = precioAplicado,
                    DescuentoUnitario = descuentoUnitario
                });

                resumenDetalles.Add(new
                {
                    ProductoId = productoId,
                    NombreProducto = producto.Nombre,
                    LoteId = lote.Id,
                    CodigoLote = lote.CodigoLote,
                    FechaVencimiento = lote.FechaVencimiento.ToString("yyyy-MM-dd"),
                    Cantidad = cantidad,
                    PrecioOriginal = lote.PrecioVenta,
                    DescuentoUnitario = descuentoUnitario,
                    PrecioAplicado = precioAplicado,
                    Subtotal = Math.Round(precioAplicado * cantidad, 2),
                    EnVentanaCritica = enVentanaCritica
                });
            }

            // Fase 2: Calcular totales de la cabecera
            decimal subtotal = detalles.Sum(d => d.PrecioAplicado * d.Cantidad + d.DescuentoUnitario * d.Cantidad);
            decimal descuentoTotal = detalles.Sum(d => d.DescuentoUnitario * d.Cantidad);
            decimal total = detalles.Sum(d => d.Subtotal);

            var cabecera = new Venta
            {
                UsuarioId = usuarioId,
                ClienteId = clienteId,
                Subtotal = Math.Round(subtotal, 2),
                DescuentoTotal = Math.Round(descuentoTotal, 2),
                Total = Math.Round(total, 2),
                FechaVenta = DateTime.Now
            };

            // Fase 3: Persistir en una sola operación atómica vía SP_CREAR_VENTA
            var ventaCreada = _ventaRepository.CrearVenta(cabecera, detalles);

            var resultado = new
            {
                ventaCreada.Id,
                ventaCreada.NumeroFactura,
                FechaVenta = ventaCreada.FechaVenta.ToString("yyyy-MM-dd HH:mm:ss"),
                UsuarioId = usuarioId,
                NombreUsuario = usuario.NombreCompleto,
                ClienteId = clienteId,
                Subtotal = cabecera.Subtotal,
                DescuentoTotal = cabecera.DescuentoTotal,
                Total = cabecera.Total,
                Detalles = resumenDetalles
            };

            return ResultadoOperacion.Exito(
                $"Venta registrada correctamente. Factura: {ventaCreada.NumeroFactura}. " +
                $"Total: ${cabecera.Total:F2}.",
                resultado);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Obtiene el encabezado y los detalles completos de una venta por su ID.
    /// Incluye el lote y producto de cada línea para trazabilidad.
    /// </summary>
    public ResultadoOperacion ObtenerVentaPorId(int id)
    {
        try
        {
            if (id <= 0)
                return ResultadoOperacion.Fallo("El identificador de la venta no es válido.");

            var venta = _ventaRepository.ObtenerPorId(id);
            if (venta is null)
                return ResultadoOperacion.Fallo(
                    $"No se encontró ninguna venta con el ID {id}.");

            var detalles = _detalleVentaRepository
                .ObtenerPorVenta(id)
                .Select(d =>
                {
                    var lote = _loteRepository.ObtenerPorId(d.LoteId);
                    var producto = lote is not null
                        ? _productoRepository.ObtenerPorId(lote.ProductoId)
                        : null;

                    return new
                    {
                        d.Id,
                        d.LoteId,
                        CodigoLote = lote?.CodigoLote ?? "N/A",
                        ProductoId = lote?.ProductoId,
                        NombreProducto = producto?.Nombre ?? "Desconocido",
                        FechaVencimiento = lote?.FechaVencimiento.ToString("yyyy-MM-dd") ?? "N/A",
                        d.Cantidad,
                        d.PrecioAplicado,
                        d.DescuentoUnitario,
                        Subtotal = d.Subtotal
                    };
                }).ToList();

            var resultado = new
            {
                venta.Id,
                venta.NumeroFactura,
                FechaVenta = venta.FechaVenta.ToString("yyyy-MM-dd HH:mm:ss"),
                venta.UsuarioId,
                venta.ClienteId,
                venta.Subtotal,
                venta.DescuentoTotal,
                venta.Total,
                venta.Anulada,
                FechaAnulacion = venta.FechaAnulacion?.ToString("yyyy-MM-dd HH:mm:ss"),
                venta.AnuladaPor,
                Detalles = detalles
            };

            return ResultadoOperacion.Exito(
                $"Venta '{venta.NumeroFactura}' encontrada.",
                resultado);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Retorna el historial de ventas realizadas por un usuario específico,
    /// excluyendo las ventas anuladas.
    /// </summary>
    public ResultadoOperacion ObtenerVentasPorUsuario(int usuarioId)
    {
        try
        {
            if (usuarioId <= 0)
                return ResultadoOperacion.Fallo("El identificador del usuario no es válido.");

            var usuario = _usuarioRepository.ObtenerPorId(usuarioId);
            if (usuario is null)
                return ResultadoOperacion.Fallo(
                    $"No se encontró ningún usuario con el ID {usuarioId}.");

            var ventas = _ventaRepository.ObtenerPorUsuario(usuarioId).ToList();

            if (ventas.Count == 0)
                return ResultadoOperacion.Exito(
                    $"El usuario '{usuario.NombreCompleto}' no tiene ventas registradas.",
                    new List<object>());

            var resultado = ventas.Select(v => new
            {
                v.Id,
                v.NumeroFactura,
                FechaVenta = v.FechaVenta.ToString("yyyy-MM-dd HH:mm:ss"),
                v.ClienteId,
                v.Subtotal,
                v.DescuentoTotal,
                v.Total,
                v.Anulada
            }).ToList();

            return ResultadoOperacion.Exito(
                $"Se encontraron {resultado.Count} venta(s) para el usuario '{usuario.NombreCompleto}'.",
                resultado);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Retorna todas las ventas registradas en un rango de fechas dado.
    /// </summary>
    public ResultadoOperacion ObtenerVentasPorFechas(DateTime desde, DateTime hasta)
    {
        try
        {
            if (desde > hasta)
                return ResultadoOperacion.Fallo(
                    "La fecha de inicio no puede ser posterior a la fecha de fin.");

            if ((hasta - desde).TotalDays > 366)
                return ResultadoOperacion.Fallo(
                    "El rango de fechas no puede superar los 366 días.");

            var ventas = _ventaRepository.ObtenerPorRangoFechas(desde, hasta).ToList();

            if (ventas.Count == 0)
                return ResultadoOperacion.Exito(
                    $"No se encontraron ventas entre {desde:yyyy-MM-dd} y {hasta:yyyy-MM-dd}.",
                    new List<object>());

            var resultado = ventas.Select(v => new
            {
                v.Id,
                v.NumeroFactura,
                FechaVenta = v.FechaVenta.ToString("yyyy-MM-dd HH:mm:ss"),
                v.UsuarioId,
                v.ClienteId,
                v.Subtotal,
                v.DescuentoTotal,
                v.Total,
                v.Anulada
            }).ToList();

            return ResultadoOperacion.Exito(
                $"Se encontraron {resultado.Count} venta(s) entre {desde:yyyy-MM-dd} y {hasta:yyyy-MM-dd}.",
                resultado);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Anula una venta existente.
    /// Verifica que la venta exista, que no esté ya anulada y que el usuario
    /// que solicita la anulación tenga permisos de administrador o farmacéutico.
    /// La reversión del stock se delega al procedimiento almacenado SP_ANULAR_VENTA.
    /// </summary>
    public ResultadoOperacion AnularVenta(int ventaId, int usuarioId)
    {
        try
        {
            if (ventaId <= 0)
                return ResultadoOperacion.Fallo("El identificador de la venta no es válido.");

            if (usuarioId <= 0)
                return ResultadoOperacion.Fallo("El identificador del usuario no es válido.");

            var venta = _ventaRepository.ObtenerPorId(ventaId);
            if (venta is null)
                return ResultadoOperacion.Fallo(
                    $"No se encontró ninguna venta con el ID {ventaId}.");

            if (venta.Anulada)
                return ResultadoOperacion.Fallo(
                    $"La venta '{venta.NumeroFactura}' ya fue anulada el " +
                    $"{venta.FechaAnulacion:yyyy-MM-dd HH:mm:ss}. No se puede anular dos veces.");

            // Solo Administrador o Farmacéutico pueden anular ventas
            var usuario = _usuarioRepository.ObtenerPorId(usuarioId);
            if (usuario is null)
                return ResultadoOperacion.Fallo(
                    $"No se encontró ningún usuario con el ID {usuarioId}.");

            if (usuario.Rol != RolUsuario.Administrador && usuario.Rol != RolUsuario.Farmaceutico)
                return ResultadoOperacion.Fallo(
                    $"El usuario '{usuario.NombreCompleto}' no tiene permisos para anular ventas. " +
                    "Solo los roles Administrador y Farmacéutico pueden realizar esta operación.");

            // Delegar anulación y reversión de stock al SP_ANULAR_VENTA
            _ventaRepository.AnularVenta(ventaId, usuarioId);

            return ResultadoOperacion.Exito(
                $"Venta '{venta.NumeroFactura}' anulada correctamente por '{usuario.NombreCompleto}'. " +
                $"El stock de los lotes involucrados ha sido restaurado.",
                new
                {
                    VentaId = ventaId,
                    NumeroFactura = venta.NumeroFactura,
                    AnuladaPor = usuario.NombreCompleto,
                    FechaAnulacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    TotalRevertido = venta.Total
                });
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }
}
