using BLL.DTOs;
using BLL.Interfaces;
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

    public LoteService(ILoteDAO loteDAO, IProductoDAO productoDAO, IParametroService parametroService)
    {
        _loteDAO = loteDAO;
        _productoDAO = productoDAO;
        _parametroService = parametroService;
    }

    public ResultadoOperacion ObtenerPorId(int id)
    {
        try
        {
            if (id <= 0) return ResultadoOperacion.Fallo("ID no válido.");
            var lote = _loteDAO.ObtenerPorId(id);
            if (lote is null) return ResultadoOperacion.Fallo($"Lote con ID {id} no encontrado.");
            return ResultadoOperacion.Exito($"Lote '{lote.CodigoLote}' encontrado.", lote);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ObtenerPorProducto(int productoId)
    {
        try
        {
            if (productoId <= 0) return ResultadoOperacion.Fallo("ID de producto no válido.");
            var producto = _productoDAO.ObtenerPorId(productoId);
            if (producto is null) return ResultadoOperacion.Fallo($"Producto con ID {productoId} no encontrado.");

            var lotes = _loteDAO.ObtenerPorProducto(productoId);
            if (lotes.Count == 0) return ResultadoOperacion.Exito($"'{producto.Nombre}' no tiene lotes activos.", new List<object>());

            int diasVentana = _parametroService.ObtenerDiasVentanaCritica();
            var resultado = lotes.Select(l => new
            {
                l.IdLote, l.CodigoLote, l.IdProducto, NombreProducto = producto.Nombre,
                FechaFabricacion = l.FechaFabricacion.ToString("yyyy-MM-dd"),
                FechaVencimiento = l.FechaVencimiento.ToString("yyyy-MM-dd"),
                l.DiasParaVencimiento, l.CantidadActual, l.CantidadInicial,
                l.PrecioCompra, l.PrecioVenta, Estado = l.Estado,
                EnVentanaCritica = l.EstaEnVentanaCritica(diasVentana), l.EstaVencido
            }).ToList();

            return ResultadoOperacion.Exito($"{resultado.Count} lote(s) para '{producto.Nombre}'.", resultado);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ObtenerTodosActivos()
    {
        try
        {
            var lotes = _loteDAO.ObtenerTodosActivos();
            if (lotes.Count == 0) return ResultadoOperacion.Exito("No hay lotes activos.", new List<object>());
            int diasVentana = _parametroService.ObtenerDiasVentanaCritica();
            var resultado = lotes.Select(l => new
            {
                l.IdLote, l.CodigoLote, l.IdProducto,
                FechaVencimiento = l.FechaVencimiento.ToString("yyyy-MM-dd"),
                l.DiasParaVencimiento, l.CantidadActual, l.PrecioCompra, l.PrecioVenta,
                Estado = l.Estado, EnVentanaCritica = l.EstaEnVentanaCritica(diasVentana)
            }).ToList();
            return ResultadoOperacion.Exito($"{resultado.Count} lote(s) activos.", resultado);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion SeleccionarFEFO(int productoId)
    {
        try
        {
            if (productoId <= 0) return ResultadoOperacion.Fallo("ID de producto no válido.");
            var lote = _loteDAO.SeleccionarLoteFEFO(productoId);
            if (lote is null) return ResultadoOperacion.Fallo($"No hay lotes activos para el producto {productoId}.");

            int diasVentana = _parametroService.ObtenerDiasVentanaCritica();
            bool enVentanaCritica = lote.EstaEnVentanaCritica(diasVentana);
            decimal porcentajeDescuento = 0m;
            decimal precioEfectivo = lote.PrecioVenta;

            if (enVentanaCritica)
            {
                var producto = _productoDAO.ObtenerPorId(productoId);
                porcentajeDescuento = producto?.UsaDescuentoPersonalizado == true ? producto.DescuentoProximidadVencimiento!.Value : _parametroService.ObtenerPorcentajeDescuentoVencimiento();
                precioEfectivo = Math.Round(lote.PrecioVenta * (1 - porcentajeDescuento), 2);
            }

            var resultado = new
            {
                LoteId = lote.IdLote, lote.CodigoLote, lote.IdProducto,
                FechaVencimiento = lote.FechaVencimiento.ToString("yyyy-MM-dd"),
                lote.DiasParaVencimiento, lote.CantidadActual,
                PrecioVentaOriginal = lote.PrecioVenta, PorcentajeDescuento = porcentajeDescuento,
                DescuentoUnitario = Math.Round(lote.PrecioVenta - precioEfectivo, 2),
                PrecioEfectivo = precioEfectivo, EnVentanaCritica = enVentanaCritica,
                MensajeDescuento = enVentanaCritica ? $"Descuento del {porcentajeDescuento:P0} aplicado." : "Sin descuento."
            };

            return ResultadoOperacion.Exito($"Lote FEFO: '{lote.CodigoLote}' (vence en {lote.DiasParaVencimiento} días).", resultado);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion InsertarLote(Lote lote)
    {
        try
        {
            if (lote is null) return ResultadoOperacion.Fallo("Datos obligatorios.");
            if (string.IsNullOrWhiteSpace(lote.CodigoLote)) return ResultadoOperacion.Fallo("Código de lote obligatorio.");
            if (lote.IdProducto <= 0) return ResultadoOperacion.Fallo("Producto no válido.");
            if (lote.FechaVencimiento <= DateTime.Today) return ResultadoOperacion.Fallo("Fecha de vencimiento debe ser posterior a hoy.");
            if (lote.FechaFabricacion >= lote.FechaVencimiento) return ResultadoOperacion.Fallo("Fecha de fabricación debe ser anterior al vencimiento.");
            if (lote.PrecioCompra <= 0) return ResultadoOperacion.Fallo("Precio de compra debe ser mayor a cero.");
            if (lote.PrecioVenta <= 0) return ResultadoOperacion.Fallo("Precio de venta debe ser mayor a cero.");
            if (lote.PrecioVenta <= lote.PrecioCompra) return ResultadoOperacion.Fallo("Precio de venta debe ser mayor al de compra.");

            var producto = _productoDAO.ObtenerPorId(lote.IdProducto);
            if (producto is null) return ResultadoOperacion.Fallo($"Producto con ID {lote.IdProducto} no encontrado.");

            _loteDAO.Insertar(lote);
            return ResultadoOperacion.Exito($"Lote '{lote.CodigoLote}' registrado para '{producto.Nombre}'.", lote);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ActualizarStock(int loteId, int cantidad, EstadoLote estado)
    {
        try
        {
            if (loteId <= 0) return ResultadoOperacion.Fallo("ID de lote no válido.");
            if (cantidad < 0) return ResultadoOperacion.Fallo("Cantidad no puede ser negativa.");

            var lote = _loteDAO.ObtenerPorId(loteId);
            if (lote is null) return ResultadoOperacion.Fallo($"Lote con ID {loteId} no encontrado.");

            _loteDAO.ActualizarStock(loteId, cantidad, estado);
            return ResultadoOperacion.Exito($"Stock del lote '{lote.CodigoLote}' actualizado a {cantidad}. Estado: {estado}.", new { LoteId = loteId, lote.CodigoLote, NuevaCantidad = cantidad, NuevoEstado = estado.ToString() });
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }
}