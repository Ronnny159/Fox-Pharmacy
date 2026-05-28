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

    public AuditoriaService(
        IHistorialParametroDAO historialParametroDAO,
        IHistorialDescuentoProductoDAO historialDescuentoDAO,
        IAjusteInventarioDAO ajusteInventarioDAO)
    {
        _historialParametroDAO = historialParametroDAO;
        _historialDescuentoDAO = historialDescuentoDAO;
        _ajusteInventarioDAO = ajusteInventarioDAO;
    }

    /// <summary>
    /// Obtiene el historial completo de cambios de un parámetro del sistema
    /// identificado por su clave (por ejemplo: "DIAS_ALERTA_VENCIMIENTO").
    /// Permite rastrear quién lo cambió, cuándo y por qué.
    /// </summary>
    public ResultadoOperacion ObtenerHistorialParametros(string clave)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(clave))
                return ResultadoOperacion.Fallo("La clave del parámetro es obligatoria.");

            var historial = _historialParametroDAO.ObtenerPorParametro(clave.Trim().ToUpper());

            if (historial is null || historial.Count == 0)
                return ResultadoOperacion.Exito(
                    $"No se encontraron registros de cambios para el parámetro '{clave}'.",
                    new List<object>());

            var resultado = historial.Select(h => new
            {
                h.Id,
                h.ClaveParametro,
                h.ValorAnterior,
                h.ValorNuevo,
                h.Motivo,
                FechaCambio = h.FechaCambio.ToString("yyyy-MM-dd HH:mm:ss"),
                h.ModificadoPorId,
                DireccionIP = h.DireccionIP ?? "No registrada"
            }).ToList();

            return ResultadoOperacion.Exito(
                $"Se encontraron {resultado.Count} registro(s) para el parámetro '{clave}'.",
                resultado);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Obtiene el historial de modificaciones de descuento para un producto específico.
    /// Incluye descuento anterior, nuevo valor, motivo y usuario responsable.
    /// </summary>
    public ResultadoOperacion ObtenerHistorialDescuentosProducto(int productoId)
    {
        try
        {
            if (productoId <= 0)
                return ResultadoOperacion.Fallo("El identificador del producto no es válido.");

            var historial = _historialDescuentoDAO.ObtenerPorProducto(productoId);

            if (historial is null || historial.Count == 0)
                return ResultadoOperacion.Exito(
                    $"No se encontraron registros de cambios de descuento para el producto {productoId}.",
                    new List<object>());

            var resultado = historial.Select(h => new
            {
                h.Id,
                h.ProductoId,
                h.CodigoProducto,
                h.NombreProducto,
                DescuentoAnterior = h.DescuentoAnterior.HasValue
                    ? $"{h.DescuentoAnterior.Value:F2}%"
                    : "Sin descuento individual",
                DescuentoNuevo = h.DescuentoNuevo.HasValue
                    ? $"{h.DescuentoNuevo.Value:F2}%"
                    : "Sin descuento individual",
                h.Accion,
                h.Motivo,
                FechaCambio = h.FechaCambio.ToString("yyyy-MM-dd HH:mm:ss"),
                h.ModificadoPorId,
                DireccionIP = h.DireccionIP ?? "No registrada"
            }).ToList();

            return ResultadoOperacion.Exito(
                $"Se encontraron {resultado.Count} registro(s) de descuento para el producto {productoId}.",
                resultado);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Genera un resumen ejecutivo de auditoría que consolida:
    /// - Total de cambios en parámetros del sistema.
    /// - Total de modificaciones de descuentos por producto.
    /// - Total de ajustes de inventario en los últimos 30 días.
    /// Útil para el panel de administración y reportes regulatorios.
    /// </summary>
    public ResultadoOperacion ObtenerResumenAuditoria()
    {
        try
        {
            var hoy = DateTime.Today;
            var desde = hoy.AddDays(-30);

            // Historial completo de parámetros
            var totalParametros = _historialParametroDAO
                .ObtenerPorRangoFechas(desde, hoy);

            // Historial completo de descuentos
            var totalDescuentos = _historialDescuentoDAO
                .ObtenerTodos();

            // Ajustes de inventario en los últimos 30 días
            var ajustesRecientes = _ajusteInventarioDAO
                .ObtenerPorRangoFechas(desde, hoy)
                .ToList();

            // Agrupar ajustes por tipo para el resumen
            var ajustesPorTipo = ajustesRecientes
                .GroupBy(a => a.Tipo.ToString())
                .Select(g => new
                {
                    Tipo = g.Key,
                    Cantidad = g.Count(),
                    TotalUnidadesAfectadas = g.Sum(a => Math.Abs(a.Cantidad))
                })
                .ToList();

            var resumen = new
            {
                FechaResumen = hoy.ToString("yyyy-MM-dd"),
                PeriodoAnalizado = $"{desde:yyyy-MM-dd} — {hoy:yyyy-MM-dd}",
                Parametros = new
                {
                    TotalCambiosUltimos30Dias = totalParametros?.Count ?? 0
                },
                Descuentos = new
                {
                    TotalModificacionesHistoricas = totalDescuentos?.Count ?? 0
                },
                AjustesInventario = new
                {
                    TotalAjustesUltimos30Dias = ajustesRecientes.Count,
                    TotalUnidadesAfectadas = ajustesRecientes.Sum(a => Math.Abs(a.Cantidad)),
                    DesglosePorTipo = ajustesPorTipo
                }
            };

            return ResultadoOperacion.Exito(
                "Resumen de auditoría generado correctamente.",
                resumen);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }
}
