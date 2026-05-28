using BLL.DTOs;
using BLL.Interfaces;
using DAL.DAO;
using DAL.Interfaces;
using Entity;

namespace BLL.Services;

/// <summary>
/// Lógica de negocio para la gestión de lotes de inventario.
/// Orquesta el algoritmo FEFO, el control de stock, la detección
/// de inflación de precios y la aplicación de descuentos por
/// proximidad de vencimiento.
/// </summary>
public class LoteService : ILoteService
{
    private readonly ILoteDAO _loteDAO;
    private readonly IProductoDAO _productoDAO;
    private readonly IParametroService _parametroService;

    public LoteService(
        ILoteDAO loteDAO,
        IProductoDAO productoDAO,
        IParametroService parametroService)
    {
        _loteDAO = loteDAO;
        _productoDAO = productoDAO;
        _parametroService = parametroService;
    }

    /// <summary>
    /// Obtiene un lote por su identificador interno.
    /// </summary>
    public ResultadoOperacion ObtenerPorId(int id)
    {
        try
        {
            if (id <= 0)
                return ResultadoOperacion.Fallo("El identificador del lote no es válido.");

            var lote = _loteDAO.ObtenerPorId(id);

            if (lote is null)
                return ResultadoOperacion.Fallo($"No se encontró ningún lote con el ID {id}.");

            return ResultadoOperacion.Exito(
                $"Lote '{lote.CodigoLote}' encontrado.",
                lote);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Obtiene todos los lotes activos pertenecientes a un producto,
    /// ordenados por fecha de vencimiento ascendente (FEFO visual).
    /// </summary>
    public ResultadoOperacion ObtenerPorProducto(int productoId)
    {
        try
        {
            if (productoId <= 0)
                return ResultadoOperacion.Fallo("El identificador del producto no es válido.");

            var producto = _productoDAO.ObtenerPorId(productoId);
            if (producto is null)
                return ResultadoOperacion.Fallo(
                    $"No se encontró ningún producto con el ID {productoId}.");

            var lotes = _loteDAO.ObtenerPorProducto(productoId).ToList();

            if (lotes.Count == 0)
                return ResultadoOperacion.Exito(
                    $"El producto '{producto.Nombre}' no tiene lotes activos registrados.",
                    new List<object>());

            int diasVentana = _parametroService.ObtenerDiasVentanaCritica();

            var resultado = lotes.Select(l => new
            {
                l.Id,
                l.CodigoLote,
                l.ProductoId,
                NombreProducto = producto.Nombre,
                FechaFabricacion = l.FechaFabricacion.ToString("yyyy-MM-dd"),
                FechaVencimiento = l.FechaVencimiento.ToString("yyyy-MM-dd"),
                l.DiasParaVencimiento,
                l.CantidadActual,
                l.CantidadInicial,
                l.PrecioCompra,
                l.PrecioVenta,
                Estado = l.Estado.ToString(),
                EnVentanaCritica = l.EstaEnVentanaCritica(diasVentana),
                EstaVencido = l.EstaVencido
            }).ToList();

            return ResultadoOperacion.Exito(
                $"Se encontraron {resultado.Count} lote(s) para el producto '{producto.Nombre}'.",
                resultado);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Obtiene todos los lotes con estado Activo en el sistema,
    /// enriquecidos con indicadores de ventana crítica.
    /// </summary>
    public ResultadoOperacion ObtenerTodosActivos()
    {
        try
        {
            var lotes = _loteDAO.ObtenerTodosActivos().ToList();

            if (lotes.Count == 0)
                return ResultadoOperacion.Exito(
                    "No hay lotes activos registrados en el sistema.",
                    new List<object>());

            int diasVentana = _parametroService.ObtenerDiasVentanaCritica();

            var resultado = lotes.Select(l => new
            {
                l.Id,
                l.CodigoLote,
                l.ProductoId,
                FechaVencimiento = l.FechaVencimiento.ToString("yyyy-MM-dd"),
                l.DiasParaVencimiento,
                l.CantidadActual,
                l.PrecioCompra,
                l.PrecioVenta,
                Estado = l.Estado.ToString(),
                EnVentanaCritica = l.EstaEnVentanaCritica(diasVentana)
            }).ToList();

            return ResultadoOperacion.Exito(
                $"Se encontraron {resultado.Count} lote(s) activos.",
                resultado);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Ejecuta el algoritmo FEFO para un producto:
    /// selecciona el lote activo con la fecha de vencimiento más próxima.
    /// Criterio de desempate: menor precio de compra.
    /// También calcula el precio de venta efectivo aplicando el descuento
    /// por proximidad de vencimiento si el lote está en ventana crítica.
    /// </summary>
    public ResultadoOperacion SeleccionarFEFO(int productoId)
    {
        try
        {
            if (productoId <= 0)
                return ResultadoOperacion.Fallo("El identificador del producto no es válido.");

            var lote = _loteDAO.SeleccionarLoteFEFO(productoId);

            if (lote is null)
                return ResultadoOperacion.Fallo(
                    $"No hay lotes activos disponibles para el producto {productoId}. " +
                    "Verifique el inventario.");

            int diasVentana = _parametroService.ObtenerDiasVentanaCritica();
            bool enVentanaCritica = lote.EstaEnVentanaCritica(diasVentana);

            // Calcular descuento aplicable según configuración del sistema
            decimal porcentajeDescuento = 0m;
            decimal precioEfectivo = lote.PrecioVenta;

            if (enVentanaCritica)
            {
                var producto = _productoDAO.ObtenerPorId(productoId);

                // Usar descuento personalizado del producto si existe, si no el general
                porcentajeDescuento = producto?.UsaDescuentoPersonalizado == true
                    ? producto.DescuentoProximidadVencimiento!.Value
                    : _parametroService.ObtenerPorcentajeDescuentoVencimiento();

                precioEfectivo = Math.Round(lote.PrecioVenta * (1 - porcentajeDescuento), 2);
            }

            var resultado = new
            {
                LoteId = lote.Id,
                lote.CodigoLote,
                lote.ProductoId,
                FechaVencimiento = lote.FechaVencimiento.ToString("yyyy-MM-dd"),
                lote.DiasParaVencimiento,
                lote.CantidadActual,
                PrecioVentaOriginal = lote.PrecioVenta,
                PorcentajeDescuento = porcentajeDescuento,
                DescuentoUnitario = Math.Round(lote.PrecioVenta - precioEfectivo, 2),
                PrecioEfectivo = precioEfectivo,
                EnVentanaCritica = enVentanaCritica,
                MensajeDescuento = enVentanaCritica
                    ? $"Descuento del {porcentajeDescuento:P0} por proximidad de vencimiento aplicado."
                    : "Sin descuento por vencimiento."
            };

            return ResultadoOperacion.Exito(
                $"Lote FEFO seleccionado: '{lote.CodigoLote}' " +
                $"(vence {lote.DiasParaVencimiento} días).",
                resultado);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Registra un nuevo lote en el inventario.
    /// Valida la integridad de los datos, verifica que el producto exista
    /// y detecta automáticamente si hubo un aumento de precio respecto
    /// al lote anterior del mismo producto (alerta de inflación).
    /// </summary>
    public ResultadoOperacion InsertarLote(Lote lote)
    {
        try
        {
            if (lote is null)
                return ResultadoOperacion.Fallo("Los datos del lote son obligatorios.");

            if (string.IsNullOrWhiteSpace(lote.CodigoLote))
                return ResultadoOperacion.Fallo("El código de lote es obligatorio.");

            if (lote.ProductoId <= 0)
                return ResultadoOperacion.Fallo("Debe asociar el lote a un producto válido.");

            if (lote.FechaVencimiento <= DateTime.Today)
                return ResultadoOperacion.Fallo(
                    "La fecha de vencimiento debe ser posterior a la fecha actual.");

            if (lote.FechaFabricacion >= lote.FechaVencimiento)
                return ResultadoOperacion.Fallo(
                    "La fecha de fabricación debe ser anterior a la fecha de vencimiento.");

            if (lote.PrecioCompra <= 0)
                return ResultadoOperacion.Fallo("El precio de compra debe ser mayor a cero.");

            if (lote.PrecioVenta <= 0)
                return ResultadoOperacion.Fallo("El precio de venta debe ser mayor a cero.");

            if (lote.PrecioVenta <= lote.PrecioCompra)
                return ResultadoOperacion.Fallo(
                    "El precio de venta debe ser mayor al precio de compra para garantizar margen positivo.");

            // Verificar que el producto exista
            var producto = _productoDAO.ObtenerPorId(lote.ProductoId);
            if (producto is null)
                return ResultadoOperacion.Fallo(
                    $"No se encontró ningún producto con el ID {lote.ProductoId}.");

            // Detectar inflación: comparar con el precio de compra del lote más reciente
            bool alertaInflacion = false;
            decimal porcentajeAumento = 0m;
            decimal precioAnterior = 0m;

            var lotesExistentes = _loteDAO
                .ObtenerPorProducto(lote.ProductoId)
                .OrderByDescending(l => l.FechaCreacion)
                .ToList();

            if (lotesExistentes.Count > 0)
            {
                var loteAnterior = lotesExistentes.First();
                precioAnterior = loteAnterior.PrecioCompra;

                if (lote.PrecioCompra > precioAnterior && precioAnterior > 0)
                {
                    alertaInflacion = true;
                    porcentajeAumento = Math.Round(
                        ((lote.PrecioCompra - precioAnterior) / precioAnterior) * 100, 2);
                }
            }

            _loteDAO.Insertar(lote);

            var mensajeBase = $"Lote '{lote.CodigoLote}' registrado correctamente " +
                              $"para el producto '{producto.Nombre}'.";

            var datos = new
            {
                lote.CodigoLote,
                lote.ProductoId,
                NombreProducto = producto.Nombre,
                FechaVencimiento = lote.FechaVencimiento.ToString("yyyy-MM-dd"),
                lote.PrecioCompra,
                lote.PrecioVenta,
                AlertaInflacion = alertaInflacion,
                PrecioAnterior = precioAnterior,
                PorcentajeAumento = porcentajeAumento,
                MensajeInflacion = alertaInflacion
                    ? $"ALERTA: El precio de compra aumentó un {porcentajeAumento}% " +
                      $"respecto al lote anterior (${precioAnterior:F2} → ${lote.PrecioCompra:F2})."
                    : string.Empty
            };

            var mensajeFinal = alertaInflacion
                ? mensajeBase + $" {datos.MensajeInflacion}"
                : mensajeBase;

            return ResultadoOperacion.Exito(mensajeFinal, datos);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Actualiza el stock y el estado de un lote existente.
    /// Usado internamente por el proceso de venta y ajustes de inventario.
    /// </summary>
    public ResultadoOperacion ActualizarStock(int loteId, int cantidad, EstadoLote estado)
    {
        try
        {
            if (loteId <= 0)
                return ResultadoOperacion.Fallo("El identificador del lote no es válido.");

            if (cantidad < 0)
                return ResultadoOperacion.Fallo(
                    "La cantidad no puede ser negativa.");

            var lote = _loteDAO.ObtenerPorId(loteId);
            if (lote is null)
                return ResultadoOperacion.Fallo(
                    $"No se encontró ningún lote con el ID {loteId}.");

            // Si la cantidad llega a cero, forzar estado Agotado
            var estadoFinal = cantidad == 0 ? EstadoLote.Agotado : estado;

            _loteDAO.ActualizarStock(loteId, cantidad, estadoFinal);

            return ResultadoOperacion.Exito(
                $"Stock del lote '{lote.CodigoLote}' actualizado a {cantidad} unidades. " +
                $"Estado: {estadoFinal}.",
                new
                {
                    LoteId = loteId,
                    lote.CodigoLote,
                    NuevaCantidad = cantidad,
                    NuevoEstado = estadoFinal.ToString()
                });
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }
}
