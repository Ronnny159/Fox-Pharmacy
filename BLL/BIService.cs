using BLL.DTOs;
using BLL.Interfaces;
using DAL.Interfaces;
using Entity;

namespace BLL.Services;

/// <summary>
/// Lógica de negocio para Business Intelligence.
/// Consolida análisis de ventas, rotación de productos,
/// proximidad de vencimiento y estado del inventario.
/// </summary>
public class BIService : IBIService
{
    private readonly IVentaDAO _ventaDAO;
    private readonly IDetalleVentaDAO _detalleVentaDAO;
    private readonly IProductoDAO _productoDAO;
    private readonly ILoteDAO _loteDAO;

    public BIService(
        IVentaDAO ventaDAO,
        IDetalleVentaDAO detalleVentaDAO,
        IProductoDAO productoDAO,
        ILoteDAO loteDAO)
    {
        _ventaDAO = ventaDAO;
        _detalleVentaDAO = detalleVentaDAO;
        _productoDAO = productoDAO;
        _loteDAO = loteDAO;
    }

    /// <summary>
    /// Obtiene el ranking de productos por volumen de unidades vendidas.
    /// Permite identificar los más y menos vendidos para decisiones de compra y promociones.
    /// </summary>
    /// <param name="top">Cantidad de productos a retornar (por defecto 10).</param>
    /// <param name="orden">
    /// "MAYOR" para los más vendidos (Top 10 rotación alta),
    /// "MENOR" para los menos vendidos (Bottom 10 estancados).
    /// </param>
    public ResultadoOperacion ObtenerTopProductos(int top = 10, string orden = "MAYOR")
    {
        try
        {
            if (top <= 0)
                return ResultadoOperacion.Fallo("El parámetro 'top' debe ser mayor a cero.");

            var ordenNormalizado = orden.Trim().ToUpper();
            if (ordenNormalizado != "MAYOR" && ordenNormalizado != "MENOR")
                return ResultadoOperacion.Fallo(
                    "El parámetro 'orden' solo acepta los valores 'MAYOR' o 'MENOR'.");

            // Obtener ventas del último año como base del análisis de rotación
            var desde = DateTime.Today.AddYears(-1);
            var hasta = DateTime.Today;

            var ventas = _ventaDAO
                .ObtenerPorRangoFechas(desde, hasta)
                .Where(v => !v.Anulada)
                .ToList();

            if (ventas.Count == 0)
                return ResultadoOperacion.Exito(
                    "No se encontraron ventas en el último año para calcular el ranking.",
                    new List<object>());

            // Reunir todos los detalles de las ventas y agrupar por producto
            var detallesPorProducto = ventas
                .SelectMany(v => _detalleVentaDAO.ObtenerPorVenta(v.Id))
                .GroupBy(d => d.Lote?.ProductoId ?? 0)
                .Where(g => g.Key > 0)
                .Select(g => new
                {
                    ProductoId = g.Key,
                    TotalUnidades = g.Sum(d => d.Cantidad),
                    TotalIngresos = g.Sum(d => d.Subtotal),
                    NumeroTransacciones = g.Select(d => d.VentaId).Distinct().Count()
                });

            // Aplicar el orden solicitado
            var ranking = ordenNormalizado == "MAYOR"
                ? detallesPorProducto.OrderByDescending(p => p.TotalUnidades).Take(top)
                : detallesPorProducto.OrderBy(p => p.TotalUnidades).Take(top);

            // Enriquecer con nombre del producto
            var resultado = ranking.Select(r =>
            {
                var producto = _productoDAO.ObtenerPorId(r.ProductoId);
                return new
                {
                    r.ProductoId,
                    CodigoProducto = producto?.Codigo ?? "N/A",
                    NombreProducto = producto?.Nombre ?? "Desconocido",
                    r.TotalUnidades,
                    TotalIngresos = Math.Round(r.TotalIngresos, 2),
                    r.NumeroTransacciones,
                    Clasificacion = ordenNormalizado == "MAYOR" ? "Alta rotación" : "Baja rotación"
                };
            }).ToList();

            return ResultadoOperacion.Exito(
                $"Top {top} productos con {(ordenNormalizado == "MAYOR" ? "mayor" : "menor")} rotación obtenido correctamente.",
                resultado);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Genera un resumen ejecutivo de ventas para un rango de fechas dado.
    /// Incluye totales, descuentos aplicados, promedio por venta y desglose diario.
    /// </summary>
    public ResultadoOperacion ObtenerResumenVentas(DateTime desde, DateTime hasta)
    {
        try
        {
            if (desde > hasta)
                return ResultadoOperacion.Fallo(
                    "La fecha de inicio no puede ser posterior a la fecha de fin.");

            if ((hasta - desde).TotalDays > 366)
                return ResultadoOperacion.Fallo(
                    "El rango de fechas no puede superar los 366 días.");

            var ventas = _ventaDAO
                .ObtenerPorRangoFechas(desde, hasta)
                .ToList();

            var ventasValidas = ventas.Where(v => !v.Anulada).ToList();
            var ventasAnuladas = ventas.Where(v => v.Anulada).ToList();

            if (ventasValidas.Count == 0)
                return ResultadoOperacion.Exito(
                    $"No se encontraron ventas en el rango {desde:yyyy-MM-dd} — {hasta:yyyy-MM-dd}.",
                    new { TotalVentas = 0, TotalIngresos = 0m });

            var totalIngresos = ventasValidas.Sum(v => v.Total);
            var totalDescuentos = ventasValidas.Sum(v => v.DescuentoTotal);
            var totalSubtotal = ventasValidas.Sum(v => v.Subtotal);
            var promedioPorVenta = ventasValidas.Count > 0
                ? Math.Round(totalIngresos / ventasValidas.Count, 2)
                : 0m;

            // Desglose por día para gráficas en la UI
            var desgloseDiario = ventasValidas
                .GroupBy(v => v.FechaVenta.Date)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Fecha = g.Key.ToString("yyyy-MM-dd"),
                    NumeroVentas = g.Count(),
                    Ingresos = Math.Round(g.Sum(v => v.Total), 2),
                    Descuentos = Math.Round(g.Sum(v => v.DescuentoTotal), 2)
                })
                .ToList();

            var resumen = new
            {
                PeriodoDesde = desde.ToString("yyyy-MM-dd"),
                PeriodoHasta = hasta.ToString("yyyy-MM-dd"),
                TotalVentas = ventasValidas.Count,
                TotalAnuladas = ventasAnuladas.Count,
                TotalSubtotal = Math.Round(totalSubtotal, 2),
                TotalDescuentos = Math.Round(totalDescuentos, 2),
                TotalIngresos = Math.Round(totalIngresos, 2),
                PorcentajeDescuento = totalSubtotal > 0
                    ? Math.Round((totalDescuentos / totalSubtotal) * 100, 2)
                    : 0m,
                PromedioPorVenta = promedioPorVenta,
                DesgloseDiario = desgloseDiario
            };

            return ResultadoOperacion.Exito(
                $"Resumen de ventas del {desde:yyyy-MM-dd} al {hasta:yyyy-MM-dd} generado correctamente.",
                resumen);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Obtiene los lotes activos cuya fecha de vencimiento cae dentro
    /// de los próximos <paramref name="dias"/> días.
    /// Base para alertas preventivas y decisiones de descuento.
    /// </summary>
    public ResultadoOperacion ObtenerProductosPorVencer(int dias = 30)
    {
        try
        {
            if (dias <= 0)
                return ResultadoOperacion.Fallo("El número de días debe ser mayor a cero.");

            var hoy = DateTime.Today;
            var limite = hoy.AddDays(dias);

            var lotesActivos = _loteDAO.ObtenerTodosActivos();

            var lotesPorVencer = lotesActivos
                .Where(l => l.FechaVencimiento.Date >= hoy
                         && l.FechaVencimiento.Date <= limite
                         && l.Estado == EstadoLote.Activo
                         && l.CantidadActual > 0)
                .OrderBy(l => l.FechaVencimiento)
                .ToList();

            if (lotesPorVencer.Count == 0)
                return ResultadoOperacion.Exito(
                    $"No hay lotes activos que venzan en los próximos {dias} días.",
                    new List<object>());

            var resultado = lotesPorVencer.Select(l =>
            {
                var producto = _productoDAO.ObtenerPorId(l.ProductoId);
                var diasRestantes = (l.FechaVencimiento.Date - hoy).Days;

                return new
                {
                    LoteId = l.Id,
                    l.CodigoLote,
                    ProductoId = l.ProductoId,
                    CodigoProducto = producto?.Codigo ?? "N/A",
                    NombreProducto = producto?.Nombre ?? "Desconocido",
                    FechaVencimiento = l.FechaVencimiento.ToString("yyyy-MM-dd"),
                    DiasRestantes = diasRestantes,
                    CantidadActual = l.CantidadActual,
                    PrecioVenta = l.PrecioVenta,
                    ValorEnRiesgo = Math.Round(l.PrecioVenta * l.CantidadActual, 2),
                    Urgencia = diasRestantes <= 7 ? "CRÍTICA"
                                      : diasRestantes <= 15 ? "ALTA"
                                                            : "MEDIA"
                };
            }).ToList();

            var valorTotalEnRiesgo = resultado.Sum(r => r.ValorEnRiesgo);

            return ResultadoOperacion.Exito(
                $"Se encontraron {resultado.Count} lote(s) por vencer en los próximos {dias} días. " +
                $"Valor total en riesgo: ${valorTotalEnRiesgo:F2}.",
                resultado);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Genera un resumen general del estado actual del inventario:
    /// lotes activos, agotados, vencidos, valor total en stock
    /// y productos bajo su nivel de stock mínimo.
    /// </summary>
    public ResultadoOperacion ObtenerResumenInventario()
    {
        try
        {
            var todosLosLotes = _loteDAO.ObtenerTodosActivos().ToList();
            var todosProductos = _productoDAO.ObtenerTodos().ToList();

            // Clasificar lotes por estado
            var lotesActivos = todosLosLotes.Where(l => l.Estado == EstadoLote.Activo).ToList();
            var lotesAgotados = todosLosLotes.Where(l => l.Estado == EstadoLote.Agotado).ToList();

            // Calcular valor total del inventario activo
            var valorTotalInventario = lotesActivos
                .Sum(l => l.PrecioVenta * l.CantidadActual);

            // Calcular valor al costo (inversión actual)
            var valorCostoInventario = lotesActivos
                .Sum(l => l.PrecioCompra * l.CantidadActual);

            // Detectar productos bajo stock mínimo
            var stockPorProducto = lotesActivos
                .GroupBy(l => l.ProductoId)
                .ToDictionary(g => g.Key, g => g.Sum(l => l.CantidadActual));

            var productosBajoMinimo = todosProductos
                .Where(p => p.StockMinimo > 0
                         && stockPorProducto.GetValueOrDefault(p.Id, 0) < p.StockMinimo)
                .Select(p => new
                {
                    p.Id,
                    p.Codigo,
                    p.Nombre,
                    StockActual = stockPorProducto.GetValueOrDefault(p.Id, 0),
                    p.StockMinimo,
                    Diferencia = p.StockMinimo - stockPorProducto.GetValueOrDefault(p.Id, 0)
                })
                .OrderByDescending(p => p.Diferencia)
                .ToList();

            var resumen = new
            {
                FechaCorte = DateTime.Today.ToString("yyyy-MM-dd"),
                TotalProductosCatalogo = todosProductos.Count,
                TotalLotesActivos = lotesActivos.Count,
                TotalLotesAgotados = lotesAgotados.Count,
                TotalUnidadesEnStock = lotesActivos.Sum(l => l.CantidadActual),
                ValorInventarioVenta = Math.Round(valorTotalInventario, 2),
                ValorInventarioCosto = Math.Round(valorCostoInventario, 2),
                MargenBrutoEstimado = valorCostoInventario > 0
                    ? Math.Round(((valorTotalInventario - valorCostoInventario)
                                  / valorCostoInventario) * 100, 2)
                    : 0m,
                ProductosBajoStockMinimo = new
                {
                    Total = productosBajoMinimo.Count,
                    Detalle = productosBajoMinimo
                }
            };

            return ResultadoOperacion.Exito(
                "Resumen de inventario generado correctamente.",
                resumen);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }
}
