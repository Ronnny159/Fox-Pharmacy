using BLL.DTOs;
using BLL.Interfaces;
using DAL.Interfaces;

namespace BLL.Services;

/// <summary>
/// Lógica de negocio para auditoría y trazabilidad del sistema.
/// Consolida el historial de cambios en parámetros, descuentos por producto
/// y ajustes de inventario en un único punto de consulta para la UI.
/// </summary>

public class AuditoriaService : IAuditoriaService
{
    private readonly IHistorialParametroDAO _historialParametroDAO;
    private readonly IHistorialDescuentoProductoDAO _historialDescuentoDAO;
    private readonly IAjusteInventarioDAO _ajusteInventarioDAO;

    public AuditoriaService(IHistorialParametroDAO historialParametroDAO, IHistorialDescuentoProductoDAO historialDescuentoDAO, IAjusteInventarioDAO ajusteInventarioDAO)
    {
        _historialParametroDAO = historialParametroDAO; _historialDescuentoDAO = historialDescuentoDAO; _ajusteInventarioDAO = ajusteInventarioDAO;
    }

    public ResultadoOperacion ObtenerHistorialParametros(string clave)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(clave)) return ResultadoOperacion.Fallo("Clave obligatoria.");
            var historial = _historialParametroDAO.ObtenerPorParametro(clave.Trim().ToUpper());
            if (historial is null || historial.Count == 0) return ResultadoOperacion.Exito($"No hay cambios para '{clave}'.", new List<object>());
            var resultado = historial.Select(h => new { h.IdHistorial, h.ClaveParametro, h.ValorAnterior, h.ValorNuevo, h.Motivo, FechaCambio = h.FechaCambio.ToString("yyyy-MM-dd HH:mm:ss"), h.IdUsuario, DireccionIP = h.DireccionIP ?? "No registrada" }).ToList();
            return ResultadoOperacion.Exito($"{resultado.Count} registro(s) para '{clave}'.", resultado);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ObtenerHistorialDescuentosProducto(int productoId)
    {
        try
        {
            if (productoId <= 0) return ResultadoOperacion.Fallo("ID de producto no válido.");
            var historial = _historialDescuentoDAO.ObtenerPorProducto(productoId);
            if (historial is null || historial.Count == 0) return ResultadoOperacion.Exito($"No hay cambios de descuento para producto {productoId}.", new List<object>());
            var resultado = historial.Select(h => new { h.IdHistorial, h.IdProducto, h.CodigoProducto, h.NombreProducto, DescuentoAnterior = h.DescuentoAnterior.HasValue ? $"{h.DescuentoAnterior.Value:P0}" : "Sin descuento", DescuentoNuevo = h.DescuentoNuevo.HasValue ? $"{h.DescuentoNuevo.Value:P0}" : "Sin descuento", h.Accion, h.Motivo, FechaCambio = h.FechaCambio.ToString("yyyy-MM-dd HH:mm:ss"), h.IdUsuario, DireccionIP = h.DireccionIP ?? "No registrada" }).ToList();
            return ResultadoOperacion.Exito($"{resultado.Count} registro(s) para producto {productoId}.", resultado);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ObtenerResumenAuditoria()
    {
        try
        {
            var hoy = DateTime.Today; var desde = hoy.AddDays(-30);
            var totalParametros = _historialParametroDAO.ObtenerPorRangoFechas(desde, hoy);
            var totalDescuentos = _historialDescuentoDAO.ObtenerTodos();
            var ajustesRecientes = _ajusteInventarioDAO.ObtenerPorRangoFechas(desde, hoy);

            var ajustesPorTipo = ajustesRecientes.GroupBy(a => a.Tipo.ToString()).Select(g => new { Tipo = g.Key, Cantidad = g.Count(), TotalUnidadesAfectadas = g.Sum(a => Math.Abs(a.Cantidad)) }).ToList();

            var resumen = new { FechaResumen = hoy.ToString("yyyy-MM-dd"), PeriodoAnalizado = $"{desde:yyyy-MM-dd} — {hoy:yyyy-MM-dd}", Parametros = new { TotalCambiosUltimos30Dias = totalParametros?.Count ?? 0 }, Descuentos = new { TotalModificacionesHistoricas = totalDescuentos?.Count ?? 0 }, AjustesInventario = new { TotalAjustesUltimos30Dias = ajustesRecientes.Count, TotalUnidadesAfectadas = ajustesRecientes.Sum(a => Math.Abs(a.Cantidad)), DesglosePorTipo = ajustesPorTipo } };
            return ResultadoOperacion.Exito("Resumen de auditoría generado.", resumen);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }
}