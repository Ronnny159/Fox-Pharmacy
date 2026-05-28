using BLL.DTOs;
using BLL.Interfaces;
using DAL.Interfaces;
using Entity;

namespace BLL.Services;

/// <summary>
/// Lógica de negocio para la gestión de parámetros de configuración del sistema.
/// Centraliza el acceso a los valores clave que controlan el comportamiento
/// del sistema (descuentos, umbrales, ventanas críticas) y garantiza
/// la trazabilidad de cada cambio mediante historial de auditoría.
/// Principio aplicado: Open/Closed — el sistema se adapta sin recompilar.
/// </summary>

public class ParametroService : IParametroService
{
    private const string ClavePorcentajeDescuento = "PORCENTAJE_DESCUENTO_VENCIMIENTO";
    private const string ClaveUmbralInflacion = "UMBRAL_ALERTA_INFLACION";
    private const string ClaveDiasVentanaCritica = "DIAS_VENTANA_CRITICA";
    private const decimal DefaultPorcentajeDescuento = 0.20m;
    private const decimal DefaultUmbralInflacion = 0.05m;
    private const int DefaultDiasVentanaCritica = 30;

    private readonly IParametroSistemaDAO _parametroDAO;
    private readonly IHistorialParametroDAO _historialParametroDAO;
    private readonly IHistorialDescuentoProductoDAO _historialDescuentoDAO;
    private readonly IProductoDAO _productoDAO;

    public ParametroService(IParametroSistemaDAO parametroDAO, IHistorialParametroDAO historialParametroDAO, IHistorialDescuentoProductoDAO historialDescuentoDAO, IProductoDAO productoDAO)
    {
        _parametroDAO = parametroDAO;
        _historialParametroDAO = historialParametroDAO;
        _historialDescuentoDAO = historialDescuentoDAO;
        _productoDAO = productoDAO;
    }

    public decimal ObtenerPorcentajeDescuentoVencimiento()
    {
        var valor = ObtenerParametro(ClavePorcentajeDescuento);
        if (decimal.TryParse(valor, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal resultado)) return resultado;
        return DefaultPorcentajeDescuento;
    }

    public decimal ObtenerUmbralAlertaInflacion()
    {
        var valor = ObtenerParametro(ClaveUmbralInflacion);
        if (decimal.TryParse(valor, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal resultado)) return resultado;
        return DefaultUmbralInflacion;
    }

    public int ObtenerDiasVentanaCritica()
    {
        var valor = ObtenerParametro(ClaveDiasVentanaCritica);
        if (int.TryParse(valor, out int resultado) && resultado > 0) return resultado;
        return DefaultDiasVentanaCritica;
    }

    public string ObtenerParametro(string clave)
    {
        if (string.IsNullOrWhiteSpace(clave)) return string.Empty;
        try { var parametro = _parametroDAO.ObtenerPorClave(clave.Trim().ToUpper()); return parametro?.Valor ?? string.Empty; }
        catch { return string.Empty; }
    }

    public ResultadoOperacion ActualizarDescuentoGeneral(decimal nuevoPorcentaje, int usuarioId, string motivo, string? ip = null)
    {
        try
        {
            if (nuevoPorcentaje <= 0 || nuevoPorcentaje > 1) return ResultadoOperacion.Fallo("Porcentaje debe estar entre 0.01 y 1.00.");
            if (usuarioId <= 0) return ResultadoOperacion.Fallo("ID de usuario no válido.");
            if (string.IsNullOrWhiteSpace(motivo)) return ResultadoOperacion.Fallo("Motivo obligatorio para auditoría.");

            var parametro = _parametroDAO.ObtenerPorClave(ClavePorcentajeDescuento);
            string valorAnterior = parametro?.Valor ?? DefaultPorcentajeDescuento.ToString("G", System.Globalization.CultureInfo.InvariantCulture);
            string valorNuevo = nuevoPorcentaje.ToString("G", System.Globalization.CultureInfo.InvariantCulture);

            if (parametro is null)
                _parametroDAO.Insertar(new ParametroSistema { Clave = ClavePorcentajeDescuento, Valor = valorNuevo, Descripcion = "Descuento en ventana crítica." });
            else
            {
                parametro.Valor = valorNuevo;
                _parametroDAO.Actualizar(parametro);
            }

            _historialParametroDAO.Insertar(new HistorialParametro
            {
                ClaveParametro = ClavePorcentajeDescuento, ValorAnterior = valorAnterior, ValorNuevo = valorNuevo,
                Motivo = motivo.Trim(), FechaCambio = DateTime.Now, IdUsuario = usuarioId, DireccionIP = ip?.Trim()
            });

            return ResultadoOperacion.Exito($"Descuento general actualizado de {decimal.Parse(valorAnterior):P0} a {nuevoPorcentaje:P0}.", new { Clave = ClavePorcentajeDescuento, ValorAnterior = valorAnterior, ValorNuevo = valorNuevo, ModificadoPor = usuarioId });
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ActualizarDescuentoProducto(int productoId, decimal? descuento, int usuarioId, string motivo, string? ip = null)
    {
        try
        {
            if (productoId <= 0) return ResultadoOperacion.Fallo("ID de producto no válido.");
            if (descuento.HasValue && (descuento.Value <= 0 || descuento.Value > 1)) return ResultadoOperacion.Fallo("Descuento debe estar entre 0.01 y 1.00, o null.");
            if (usuarioId <= 0) return ResultadoOperacion.Fallo("ID de usuario no válido.");
            if (string.IsNullOrWhiteSpace(motivo)) return ResultadoOperacion.Fallo("Motivo obligatorio para auditoría.");

            var producto = _productoDAO.ObtenerPorId(productoId);
            if (producto is null) return ResultadoOperacion.Fallo($"Producto con ID {productoId} no encontrado.");

            decimal? descuentoAnterior = producto.DescuentoProximidadVencimiento;
            string accion = descuento.HasValue ? (descuentoAnterior.HasValue ? "MODIFICAR" : "CREAR") : "ELIMINAR";

            _productoDAO.ActualizarDescuentoIndividual(productoId, descuento);

            _historialDescuentoDAO.Insertar(new HistorialDescuentoProducto
            {
                IdProducto = productoId, CodigoProducto = producto.Codigo, NombreProducto = producto.Nombre,
                DescuentoAnterior = descuentoAnterior, DescuentoNuevo = descuento, Accion = accion,
                Motivo = motivo.Trim(), FechaCambio = DateTime.Now, IdUsuario = usuarioId, DireccionIP = ip?.Trim()
            });

            string mensajeAnt = descuentoAnterior.HasValue ? $"{descuentoAnterior.Value:P0}" : "Sin descuento";
            string mensajeNuevo = descuento.HasValue ? $"{descuento.Value:P0}" : "Sin descuento (usará general)";
            return ResultadoOperacion.Exito($"Descuento de '{producto.Nombre}': {mensajeAnt} → {mensajeNuevo}.", new { ProductoId = productoId, producto.Nombre, Accion = accion, DescuentoAnterior = descuentoAnterior, DescuentoNuevo = descuento });
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }
}