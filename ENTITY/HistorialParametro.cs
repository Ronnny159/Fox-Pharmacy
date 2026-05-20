using System;

namespace Entity;

/// <summary>
/// Registro histórico de cambios en parámetros del sistema.
/// Garantiza trazabilidad completa para auditorías internas y regulatorias.
/// </summary>
public class HistorialParametro : BaseEntity
{
    /// <summary>
    /// Clave del parámetro que fue modificado.
    /// </summary>
    public string ClaveParametro { get; set; } = string.Empty;

    /// <summary>
    /// Valor del parámetro antes del cambio.
    /// </summary>
    public string ValorAnterior { get; set; } = string.Empty;

    /// <summary>
    /// Nuevo valor asignado al parámetro.
    /// </summary>
    public string ValorNuevo { get; set; } = string.Empty;

    /// <summary>
    /// Motivo del cambio. Obligatorio para cumplimiento regulatorio.
    /// </summary>
    public string Motivo { get; set; } = string.Empty;

    /// <summary>
    /// Fecha y hora exacta en que se realizó el cambio.
    /// </summary>
    public DateTime FechaCambio { get; set; } = DateTime.Now;

    /// <summary>
    /// Identificador del usuario que realizó la modificación.
    /// </summary>
    public int ModificadoPorId { get; set; }

    /// <summary>
    /// Dirección IP desde la que se realizó el cambio (opcional pero recomendado).
    /// </summary>
    public string? DireccionIP { get; set; }

    // Propiedad navegacional
    public virtual Usuario ModificadoPor { get; set; } = null!;
}
