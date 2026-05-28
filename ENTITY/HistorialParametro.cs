using System;

namespace Entity;

/// <summary>
/// Registro histórico de cambios en parámetros del sistema.
/// Garantiza trazabilidad completa para auditorías internas y regulatorias.
/// </summary>

public class HistorialParametro : BaseEntity
{
    public int IdHistorial { get; set; }
    public string ClaveParametro { get; set; } = string.Empty;
    public string ValorAnterior { get; set; } = string.Empty;
    public string ValorNuevo { get; set; } = string.Empty;
    public string Motivo { get; set; } = string.Empty;
    public DateTime FechaCambio { get; set; } = DateTime.Now;
    public int IdUsuario { get; set; }
    public string? DireccionIP { get; set; }

    public virtual Usuario ModificadoPor { get; set; } = null!;
}