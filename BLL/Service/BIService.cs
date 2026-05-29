using BLL.DTOs;
using BLL.Interfaces;
using DAL.Interfaces;
using Entity;

namespace BLL.Services;

public class BIService : IBIService
{
    private readonly IVentaDAO _ventaDAO;
    private readonly IDetalleVentaDAO _detalleVentaDAO;
    private readonly IProductoDAO _productoDAO;
    private readonly ILoteDAO _loteDAO;

    public BIService(IVentaDAO ventaDAO, IDetalleVentaDAO detalleVentaDAO, IProductoDAO productoDAO, ILoteDAO loteDAO)
    {
        _ventaDAO = ventaDAO; _detalleVentaDAO = detalleVentaDAO; _productoDAO = productoDAO; _loteDAO = loteDAO;
    }

    public ResultadoOperacion ObtenerTopProductos(int top = 10, string orden = "MAYOR")
    {
        try
        {
            if (top <= 0) return ResultadoOperacion.Fallo("'top' debe ser mayor a cero.");
            var ordenNorm = orden.Trim().ToUpper();
            if (ordenNorm != "MAYOR" && ordenNorm != "MENOR") return ResultadoOperacion.Fallo("'orden' solo acepta 'MAYOR' o 'MENOR'.");

            var desde = DateTime.Today.AddYears(-1); var hasta = DateTime.Today;
            var ventas = _ventaDAO.ObtenerPorRangoFechas(desde, hasta).Where(v => v.Estado == 'A').ToList();
            if (ventas.Count == 0) return ResultadoOperacion.Exito("No hay ventas en el último año.", new List<object>());

            var detallesPorProducto = ventas.SelectMany(v => _detalleVentaDAO.ObtenerPorVenta(v.IdVenta))
                .GroupBy(d => d.IdProducto).Where(g => g.Key > 0)
                .Select(g => new { ProductoId = g.Key, TotalUnidades = g.Sum(d => d.Cantidad), TotalIngresos = g.Sum(d => d.Subtotal), NumTransacciones = g.Select(d => d.IdVenta).Distinct().Count() });

            var ranking = ordenNorm == "MAYOR" ? detallesPorProducto.OrderByDescending(p => p.TotalUnidades).Take(top) : detallesPorProducto.OrderBy(p => p.TotalUnidades).Take(top);

            var resultado = ranking.Select(r => { var p = _productoDAO.ObtenerPorId(r.ProductoId); return new { r.ProductoId, CodigoProducto = p?.Codigo ?? "N/A", NombreProducto = p?.Nombre ?? "Desconocido", r.TotalUnidades, TotalIngresos = Math.Round(r.TotalIngresos, 2), r.NumTransacciones, Clasificacion = ordenNorm == "MAYOR" ? "Alta rotación" : "Baja rotación" }; }).ToList();

            return ResultadoOperacion.Exito($"Top {top} productos con {(ordenNorm == "MAYOR" ? "mayor" : "menor")} rotación.", resultado);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ObtenerResumenVentas(DateTime desde, DateTime hasta)
    {
        try
        {
            if (desde > hasta) return ResultadoOperacion.Fallo("Fecha inicio no puede ser posterior a fecha fin.");
            if ((hasta - desde).TotalDays > 366) return ResultadoOperacion.Fallo("Rango máximo: 366 días.");

            var ventas = _ventaDAO.ObtenerPorRangoFechas(desde, hasta);
            var validas = ventas.Where(v => v.Estado == 'A').ToList();
            var anuladas = ventas.Where(v => v.Estado == 'N').ToList();

            if (validas.Count == 0) return ResultadoOperacion.Exito($"No hay ventas entre {desde:yyyy-MM-dd} y {hasta:yyyy-MM-dd}.", new { TotalVentas = 0, TotalIngresos = 0m });

            var totalIngresos = validas.Sum(v => v.Total); var totalDescuentos = validas.Sum(v => v.DescuentoTotal);
            var totalSubtotal = validas.Sum(v => v.Subtotal); var promedio = Math.Round(totalIngresos / validas.Count, 2);

            var desgloseDiario = validas.GroupBy(v => v.FechaVenta.Date).OrderBy(g => g.Key)
                .Select(g => new { Fecha = g.Key.ToString("yyyy-MM-dd"), NumeroVentas = g.Count(), Ingresos = Math.Round(g.Sum(v => v.Total), 2), Descuentos = Math.Round(g.Sum(v => v.DescuentoTotal), 2) }).ToList();

            var resumen = new { PeriodoDesde = desde.ToString("yyyy-MM-dd"), PeriodoHasta = hasta.ToString("yyyy-MM-dd"), TotalVentas = validas.Count, TotalAnuladas = anuladas.Count, TotalSubtotal = Math.Round(totalSubtotal, 2), TotalDescuentos = Math.Round(totalDescuentos, 2), TotalIngresos = Math.Round(totalIngresos, 2), PorcentajeDescuento = totalSubtotal > 0 ? Math.Round((totalDescuentos / totalSubtotal) * 100, 2) : 0m, PromedioPorVenta = promedio, DesgloseDiario = desgloseDiario };
            return ResultadoOperacion.Exito($"Resumen de ventas generado.", resumen);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ObtenerProductosPorVencer(int dias = 30)
    {
        try
        {
            if (dias <= 0) return ResultadoOperacion.Fallo("Días debe ser mayor a cero.");
            var hoy = DateTime.Today; var limite = hoy.AddDays(dias);
            var lotes = _loteDAO.ObtenerTodosActivos().Where(l => l.FechaVencimiento.Date >= hoy && l.FechaVencimiento.Date <= limite && l.Estado == 'A' && l.CantidadActual > 0).OrderBy(l => l.FechaVencimiento).ToList();
            if (lotes.Count == 0) return ResultadoOperacion.Exito($"No hay lotes que venzan en {dias} días.", new List<object>());

            var resultado = lotes.Select(l => { var p = _productoDAO.ObtenerPorId(l.IdProducto); var diasRest = (l.FechaVencimiento.Date - hoy).Days; return new { LoteId = l.IdLote, l.CodigoLote, ProductoId = l.IdProducto, CodigoProducto = p?.Codigo ?? "N/A", NombreProducto = p?.Nombre ?? "Desconocido", FechaVencimiento = l.FechaVencimiento.ToString("yyyy-MM-dd"), DiasRestantes = diasRest, CantidadActual = l.CantidadActual, PrecioVenta = l.PrecioVenta, ValorEnRiesgo = Math.Round(l.PrecioVenta * l.CantidadActual, 2), Urgencia = diasRest <= 7 ? "CRÍTICA" : diasRest <= 15 ? "ALTA" : "MEDIA" }; }).ToList();

            var valorTotal = resultado.Sum(r => r.ValorEnRiesgo);
            return ResultadoOperacion.Exito($"{resultado.Count} lote(s) por vencer. Valor en riesgo: ${valorTotal:F2}.", resultado);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ObtenerResumenInventario()
    {
        try
        {
            var lotes = _loteDAO.ObtenerTodosActivos();
            var productos = _productoDAO.ObtenerTodos();
            var activos = lotes.Where(l => l.Estado == 'A').ToList();
            var agotados = lotes.Where(l => l.Estado == 'B').ToList();
            var valorVenta = activos.Sum(l => l.PrecioVenta * l.CantidadActual);
            var valorCosto = activos.Sum(l => l.PrecioCompra * l.CantidadActual);
            var stockPorProducto = activos.GroupBy(l => l.IdProducto).ToDictionary(g => g.Key, g => g.Sum(l => l.CantidadActual));
            var bajoMinimo = productos.Where(p => p.StockMinimo > 0 && stockPorProducto.GetValueOrDefault(p.IdProducto, 0) < p.StockMinimo).Select(p => new { p.IdProducto, p.Codigo, p.Nombre, StockActual = stockPorProducto.GetValueOrDefault(p.IdProducto, 0), p.StockMinimo, Diferencia = p.StockMinimo - stockPorProducto.GetValueOrDefault(p.IdProducto, 0) }).OrderByDescending(p => p.Diferencia).ToList();

            var resumen = new { FechaCorte = DateTime.Today.ToString("yyyy-MM-dd"), TotalProductosCatalogo = productos.Count, TotalLotesActivos = activos.Count, TotalLotesAgotados = agotados.Count, TotalUnidadesEnStock = activos.Sum(l => l.CantidadActual), ValorInventarioVenta = Math.Round(valorVenta, 2), ValorInventarioCosto = Math.Round(valorCosto, 2), MargenBrutoEstimado = valorCosto > 0 ? Math.Round(((valorVenta - valorCosto) / valorCosto) * 100, 2) : 0m, ProductosBajoStockMinimo = new { Total = bajoMinimo.Count, Detalle = bajoMinimo } };
            return ResultadoOperacion.Exito("Resumen de inventario generado.", resumen);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }
}