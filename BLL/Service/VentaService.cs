using BLL.DTOs;
using BLL.Interfaces;
using DAL.Interfaces;
using Entity;

namespace BLL.Services;

public class VentaService : IVentaService
{
    private readonly IVentaDAO _ventaDAO;
    private readonly IDetalleVentaDAO _detalleVentaDAO;
    private readonly ILoteDAO _loteDAO;
    private readonly IProductoDAO _productoDAO;
    private readonly IUsuarioDAO _usuarioDAO;
    private readonly IClienteDAO _clienteDAO;
    private readonly IParametroService _parametroService;

    public VentaService(
        IVentaDAO ventaDAO, IDetalleVentaDAO detalleVentaDAO, ILoteDAO loteDAO,
        IProductoDAO productoDAO, IUsuarioDAO usuarioDAO, IClienteDAO clienteDAO,
        IParametroService parametroService)
    {
        _ventaDAO = ventaDAO;
        _detalleVentaDAO = detalleVentaDAO;
        _loteDAO = loteDAO;
        _productoDAO = productoDAO;
        _usuarioDAO = usuarioDAO;
        _clienteDAO = clienteDAO;
        _parametroService = parametroService;
    }

    public ResultadoOperacion RealizarVenta(List<(int productoId, int cantidad)> items, int usuarioId, int? clienteId = null)
    {
        try
        {
            if (items is null || items.Count == 0)
                return ResultadoOperacion.Fallo("La venta debe contener al menos un producto.");
            if (usuarioId <= 0)
                return ResultadoOperacion.Fallo("El identificador del usuario cajero no es válido.");

            var usuario = _usuarioDAO.ObtenerPorId(usuarioId);
            if (usuario is null) return ResultadoOperacion.Fallo($"No se encontró el usuario con ID {usuarioId}.");

            if (clienteId.HasValue)
            {
                var cliente = _clienteDAO.ObtenerPorId(clienteId.Value);
                if (cliente is null) return ResultadoOperacion.Fallo($"No se encontró el cliente con ID {clienteId.Value}.");
            }

            var duplicados = items.GroupBy(i => i.productoId).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            if (duplicados.Count > 0)
                return ResultadoOperacion.Fallo($"Productos duplicados (IDs: {string.Join(", ", duplicados)}).");

            foreach (var (productoId, cantidad) in items)
            {
                if (productoId <= 0) return ResultadoOperacion.Fallo($"ID de producto '{productoId}' no válido.");
                if (cantidad <= 0) return ResultadoOperacion.Fallo($"Cantidad para producto {productoId} debe ser mayor a cero.");
            }

            int diasVentana = _parametroService.ObtenerDiasVentanaCritica();
            decimal descuentoGeneral = _parametroService.ObtenerPorcentajeDescuentoVencimiento();
            var detalles = new List<DetalleVenta>();
            var resumenDetalles = new List<object>();

            foreach (var (productoId, cantidad) in items)
            {
                var producto = _productoDAO.ObtenerPorId(productoId);
                if (producto is null) return ResultadoOperacion.Fallo($"Producto con ID {productoId} no encontrado.");

                var lote = _loteDAO.SeleccionarLoteFEFO(productoId);
                if (lote is null) return ResultadoOperacion.Fallo($"No hay lotes activos para '{producto.Nombre}'.");
                if (lote.CantidadActual < cantidad)
                    return ResultadoOperacion.Fallo($"Stock insuficiente para '{producto.Nombre}'. Disponible: {lote.CantidadActual}, solicitado: {cantidad}.");

                decimal porcentajeDescuento = 0m;
                bool enVentanaCritica = lote.EstaEnVentanaCritica(diasVentana);

                if (enVentanaCritica)
                    porcentajeDescuento = producto.UsaDescuentoPersonalizado ? producto.DescuentoProximidadVencimiento!.Value : descuentoGeneral;

                decimal descuentoUnitario = Math.Round(lote.PrecioVenta * porcentajeDescuento, 2);
                decimal precioAplicado = lote.PrecioVenta - descuentoUnitario;

                detalles.Add(new DetalleVenta
                {
                    IdLote = lote.IdLote,
                    IdProducto = productoId,
                    Cantidad = cantidad,
                    PrecioAplicado = precioAplicado,
                    DescuentoUnitario = descuentoUnitario
                });

                resumenDetalles.Add(new
                {
                    ProductoId = productoId,
                    NombreProducto = producto.Nombre,
                    LoteId = lote.IdLote,
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

            decimal subtotal = detalles.Sum(d => d.PrecioAplicado * d.Cantidad + d.DescuentoUnitario * d.Cantidad);
            decimal descuentoTotal = detalles.Sum(d => d.DescuentoUnitario * d.Cantidad);
            decimal total = detalles.Sum(d => d.Subtotal);

            var cabecera = new Venta
            {
                IdUsuario = usuarioId,
                IdCliente = clienteId,
                Subtotal = Math.Round(subtotal, 2),
                DescuentoTotal = Math.Round(descuentoTotal, 2),
                Total = Math.Round(total, 2),
                FechaVenta = DateTime.Now
            };

            var ventaCreada = _ventaDAO.CrearVenta(cabecera, detalles);

            var resultado = new
            {
                ventaCreada.IdVenta,
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

            return ResultadoOperacion.Exito($"Venta registrada. Factura: {ventaCreada.NumeroFactura}. Total: ${cabecera.Total:F2}.", resultado);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ObtenerVentaPorId(int id)
    {
        try
        {
            if (id <= 0) return ResultadoOperacion.Fallo("ID de venta no válido.");
            var venta = _ventaDAO.ObtenerPorId(id);
            if (venta is null) return ResultadoOperacion.Fallo($"Venta con ID {id} no encontrada.");

            var detalles = _detalleVentaDAO.ObtenerPorVenta(id).Select(d =>
            {
                var lote = _loteDAO.ObtenerPorId(d.IdLote);
                var producto = lote is not null ? _productoDAO.ObtenerPorId(lote.IdProducto) : null;
                return new
                {
                    d.IdDetalle,
                    d.IdLote,
                    CodigoLote = lote?.CodigoLote ?? "N/A",
                    ProductoId = lote?.IdProducto,
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
                venta.IdVenta,
                venta.NumeroFactura,
                FechaVenta = venta.FechaVenta.ToString("yyyy-MM-dd HH:mm:ss"),
                venta.IdUsuario,
                venta.IdCliente,
                venta.Subtotal,
                venta.DescuentoTotal,
                venta.Total,
                Anulada = venta.Estado == 'N',
                FechaAnulacion = venta.FechaAnulacion?.ToString("yyyy-MM-dd HH:mm:ss"),
                venta.AnuladaPor,
                Detalles = detalles
            };

            return ResultadoOperacion.Exito($"Venta '{venta.NumeroFactura}' encontrada.", resultado);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ObtenerVentasPorUsuario(int usuarioId)
    {
        try
        {
            if (usuarioId <= 0) return ResultadoOperacion.Fallo("ID de usuario no válido.");
            var usuario = _usuarioDAO.ObtenerPorId(usuarioId);
            if (usuario is null) return ResultadoOperacion.Fallo($"Usuario con ID {usuarioId} no encontrado.");

            var ventas = _ventaDAO.ObtenerPorUsuario(usuarioId);
            if (ventas.Count == 0) return ResultadoOperacion.Exito($"El usuario '{usuario.NombreCompleto}' no tiene ventas.", new List<object>());

            var resultado = ventas.Select(v => new
            {
                v.IdVenta, v.NumeroFactura, FechaVenta = v.FechaVenta.ToString("yyyy-MM-dd HH:mm:ss"),
                v.IdCliente, v.Subtotal, v.DescuentoTotal, v.Total, Anulada = v.Estado == 'N'
            }).ToList();

            return ResultadoOperacion.Exito($"{resultado.Count} venta(s) para '{usuario.NombreCompleto}'.", resultado);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ObtenerVentasPorFechas(DateTime desde, DateTime hasta)
    {
        try
        {
            if (desde > hasta) return ResultadoOperacion.Fallo("Fecha inicio no puede ser posterior a fecha fin.");
            if ((hasta - desde).TotalDays > 366) return ResultadoOperacion.Fallo("Rango máximo: 366 días.");

            var ventas = _ventaDAO.ObtenerPorRangoFechas(desde, hasta);
            if (ventas.Count == 0) return ResultadoOperacion.Exito($"No hay ventas entre {desde:yyyy-MM-dd} y {hasta:yyyy-MM-dd}.", new List<object>());

            var resultado = ventas.Select(v => new
            {
                v.IdVenta, v.NumeroFactura, FechaVenta = v.FechaVenta.ToString("yyyy-MM-dd HH:mm:ss"),
                v.IdUsuario, v.IdCliente, v.Subtotal, v.DescuentoTotal, v.Total, Anulada = v.Estado == 'N'
            }).ToList();

            return ResultadoOperacion.Exito($"{resultado.Count} venta(s) encontradas.", resultado);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion AnularVenta(int ventaId, int usuarioId)
    {
        try
        {
            if (ventaId <= 0) return ResultadoOperacion.Fallo("ID de venta no válido.");
            if (usuarioId <= 0) return ResultadoOperacion.Fallo("ID de usuario no válido.");

            var venta = _ventaDAO.ObtenerPorId(ventaId);
            if (venta is null) return ResultadoOperacion.Fallo($"Venta con ID {ventaId} no encontrada.");
            if (venta.Estado == 'N') return ResultadoOperacion.Fallo($"La venta '{venta.NumeroFactura}' ya fue anulada.");

            var usuario = _usuarioDAO.ObtenerPorId(usuarioId);
            if (usuario is null) return ResultadoOperacion.Fallo($"Usuario con ID {usuarioId} no encontrado.");
            if (usuario.Rol != '1' && usuario.Rol != '3')
                return ResultadoOperacion.Fallo("Solo Administrador o Farmacéutico pueden anular ventas.");

            _ventaDAO.AnularVenta(ventaId, usuarioId);

            return ResultadoOperacion.Exito($"Venta '{venta.NumeroFactura}' anulada por '{usuario.NombreCompleto}'. Stock restaurado.", new
            {
                VentaId = ventaId,
                NumeroFactura = venta.NumeroFactura,
                AnuladaPor = usuario.NombreCompleto,
                FechaAnulacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                TotalRevertido = venta.Total
            });
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }
}