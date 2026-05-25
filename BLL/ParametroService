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
    // Claves canónicas de los parámetros almacenados en base de datos
    private const string ClavePorcentajeDescuento = "PORCENTAJE_DESCUENTO_VENCIMIENTO";
    private const string ClaveUmbralInflacion = "UMBRAL_ALERTA_INFLACION";
    private const string ClaveDiasVentanaCritica = "DIAS_VENTANA_CRITICA";

    // Valores por defecto si el parámetro no existe en la base de datos
    private const decimal DefaultPorcentajeDescuento = 0.20m; // 20%
    private const decimal DefaultUmbralInflacion = 0.05m; // 5%
    private const int DefaultDiasVentanaCritica = 30;

    private readonly IParametroSistemaRepository _parametroRepository;
    private readonly IHistorialParametroRepository _historialParametroRepository;
    private readonly IHistorialDescuentoProductoRepository _historialDescuentoRepository;
    private readonly IProductoRepository _productoRepository;

    public ParametroService(
        IParametroSistemaRepository parametroRepository,
        IHistorialParametroRepository historialParametroRepository,
        IHistorialDescuentoProductoRepository historialDescuentoRepository,
        IProductoRepository productoRepository)
    {
        _parametroRepository = parametroRepository;
        _historialParametroRepository = historialParametroRepository;
        _historialDescuentoRepository = historialDescuentoRepository;
        _productoRepository = productoRepository;
    }

    /// <summary>
    /// Retorna el porcentaje de descuento global a aplicar cuando un lote
    /// entra en ventana crítica de vencimiento.
    /// Valor entre 0.00 (0%) y 1.00 (100%). Por defecto: 0.20 (20%).
    /// </summary>
    public decimal ObtenerPorcentajeDescuentoVencimiento()
    {
        var valor = ObtenerParametro(ClavePorcentajeDescuento);

        if (decimal.TryParse(valor, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal resultado))
            return resultado;

        return DefaultPorcentajeDescuento;
    }

    /// <summary>
    /// Retorna el umbral porcentual a partir del cual se dispara una alerta
    /// de inflación al ingresar un nuevo lote.
    /// Valor entre 0.00 (0%) y 1.00 (100%). Por defecto: 0.05 (5%).
    /// </summary>
    public decimal ObtenerUmbralAlertaInflacion()
    {
        var valor = ObtenerParametro(ClaveUmbralInflacion);

        if (decimal.TryParse(valor, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal resultado))
            return resultado;

        return DefaultUmbralInflacion;
    }

    /// <summary>
    /// Retorna la cantidad de días antes del vencimiento que define
    /// la ventana crítica de un lote.
    /// Por defecto: 30 días.
    /// </summary>
    public int ObtenerDiasVentanaCritica()
    {
        var valor = ObtenerParametro(ClaveDiasVentanaCritica);

        if (int.TryParse(valor, out int resultado) && resultado > 0)
            return resultado;

        return DefaultDiasVentanaCritica;
    }

    /// <summary>
    /// Retorna el valor en texto de cualquier parámetro del sistema por su clave.
    /// Si el parámetro no existe retorna una cadena vacía; nunca lanza excepción.
    /// </summary>
    public string ObtenerParametro(string clave)
    {
        if (string.IsNullOrWhiteSpace(clave))
            return string.Empty;

        try
        {
            var parametro = _parametroRepository.ObtenerPorClave(clave.Trim().ToUpper());
            return parametro?.Valor ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Actualiza el porcentaje de descuento global por proximidad de vencimiento.
    /// Registra el cambio en el historial de auditoría con usuario, motivo e IP.
    /// El valor debe estar entre 0.01 (1%) y 1.00 (100%).
    /// </summary>
    public ResultadoOperacion ActualizarDescuentoGeneral(
        decimal nuevoPorcentaje,
        int usuarioId,
        string motivo,
        string? ip = null)
    {
        try
        {
            if (nuevoPorcentaje <= 0 || nuevoPorcentaje > 1)
                return ResultadoOperacion.Fallo(
                    "El porcentaje de descuento debe estar entre 0.01 (1%) y 1.00 (100%).");

            if (usuarioId <= 0)
                return ResultadoOperacion.Fallo("El identificador del usuario no es válido.");

            if (string.IsNullOrWhiteSpace(motivo))
                return ResultadoOperacion.Fallo(
                    "El motivo del cambio es obligatorio para el registro de auditoría.");

            var parametro = _parametroRepository.ObtenerPorClave(ClavePorcentajeDescuento);
            string valorAnterior = parametro?.Valor ?? DefaultPorcentajeDescuento.ToString("G", System.Globalization.CultureInfo.InvariantCulture);
            string valorNuevo = nuevoPorcentaje.ToString("G", System.Globalization.CultureInfo.InvariantCulture);

            if (parametro is null)
            {
                _parametroRepository.Insertar(new ParametroSistema
                {
                    Clave = ClavePorcentajeDescuento,
                    Valor = valorNuevo,
                    Descripcion = "Porcentaje de descuento aplicado a lotes en ventana crítica de vencimiento."
                });
            }
            else
            {
                parametro.Valor = valorNuevo;
                _parametroRepository.Actualizar(parametro);
            }

            // Registrar en historial de auditoría
            _historialParametroRepository.Insertar(new HistorialParametro
            {
                ClaveParametro = ClavePorcentajeDescuento,
                ValorAnterior = valorAnterior,
                ValorNuevo = valorNuevo,
                Motivo = motivo.Trim(),
                FechaCambio = DateTime.Now,
                ModificadoPorId = usuarioId,
                DireccionIP = ip?.Trim()
            });

            return ResultadoOperacion.Exito(
                $"Descuento general actualizado de {decimal.Parse(valorAnterior, System.Globalization.CultureInfo.InvariantCulture):P0} " +
                $"a {nuevoPorcentaje:P0} correctamente.",
                new
                {
                    Clave = ClavePorcentajeDescuento,
                    ValorAnterior = valorAnterior,
                    ValorNuevo = valorNuevo,
                    ModificadoPor = usuarioId,
                    FechaCambio = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Establece o elimina un descuento personalizado para un producto específico.
    /// Pasar <c>null</c> en <paramref name="descuento"/> elimina el descuento individual
    /// y el producto vuelve a usar el descuento general del sistema.
    /// Registra el cambio en el historial de auditoría.
    /// </summary>
    public ResultadoOperacion ActualizarDescuentoProducto(
        int productoId,
        decimal? descuento,
        int usuarioId,
        string motivo,
        string? ip = null)
    {
        try
        {
            if (productoId <= 0)
                return ResultadoOperacion.Fallo("El identificador del producto no es válido.");

            if (descuento.HasValue && (descuento.Value <= 0 || descuento.Value > 1))
                return ResultadoOperacion.Fallo(
                    "El porcentaje de descuento debe estar entre 0.01 (1%) y 1.00 (100%), " +
                    "o null para eliminar el descuento individual.");

            if (usuarioId <= 0)
                return ResultadoOperacion.Fallo("El identificador del usuario no es válido.");

            if (string.IsNullOrWhiteSpace(motivo))
                return ResultadoOperacion.Fallo(
                    "El motivo del cambio es obligatorio para el registro de auditoría.");

            var producto = _productoRepository.ObtenerPorId(productoId);
            if (producto is null)
                return ResultadoOperacion.Fallo(
                    $"No se encontró ningún producto con el ID {productoId}.");

            decimal? descuentoAnterior = producto.DescuentoProximidadVencimiento;

            // Determinar la acción para el historial
            string accion = descuento.HasValue
                ? (descuentoAnterior.HasValue ? "MODIFICAR" : "CREAR")
                : "ELIMINAR";

            _productoRepository.ActualizarDescuentoIndividual(productoId, descuento);

            // Registrar en historial de descuentos
            _historialDescuentoRepository.Insertar(new HistorialDescuentoProducto
            {
                ProductoId = productoId,
                CodigoProducto = producto.Codigo,
                NombreProducto = producto.Nombre,
                DescuentoAnterior = descuentoAnterior,
                DescuentoNuevo = descuento,
                Accion = accion,
                Motivo = motivo.Trim(),
                FechaCambio = DateTime.Now,
                ModificadoPorId = usuarioId,
                DireccionIP = ip?.Trim()
            });

            string mensajeAnterior = descuentoAnterior.HasValue
                ? $"{descuentoAnterior.Value:P0}"
                : "Sin descuento individual";

            string mensajeNuevo = descuento.HasValue
                ? $"{descuento.Value:P0}"
                : "Sin descuento individual (usará el descuento general)";

            return ResultadoOperacion.Exito(
                $"Descuento del producto '{producto.Nombre}' actualizado: " +
                $"{mensajeAnterior} → {mensajeNuevo}.",
                new
                {
                    ProductoId = productoId,
                    NombreProducto = producto.Nombre,
                    Accion = accion,
                    DescuentoAnterior = descuentoAnterior,
                    DescuentoNuevo = descuento,
                    ModificadoPor = usuarioId,
                    FechaCambio = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }
}
