using System;

namespace Entity;

/// <summary>
/// Representa una alerta o notificación enviada a un paciente
/// del programa de fidelización a través del Bot de Telegram.
/// </summary>
public class AlertaFidelizacion : BaseEntity
{
    /// <summary>
    /// Identificador del cliente que recibe la alerta.
    /// </summary>
    public int ClienteId { get; set; }

    /// <summary>
    /// Identificador del producto relacionado con la alerta (opcional).
    /// </summary>
    public int? ProductoId { get; set; }

    /// <summary>
    /// Identificador del lote relacionado con la alerta (opcional).
    /// </summary>
    public int? LoteId { get; set; }

    /// <summary>
    /// Tipo de alerta: VENCIMIENTO_PROXIMO, REPOSICION, STOCK_BAJO, PERSONALIZADA.
    /// </summary>
    public string TipoAlerta { get; set; } = string.Empty;

    /// <summary>
    /// Contenido del mensaje enviado al paciente.
    /// </summary>
    public string Mensaje { get; set; } = string.Empty;

    /// <summary>
    /// Fecha y hora en que se envió la notificación.
    /// </summary>
    public DateTime FechaEnvio { get; set; } = DateTime.Now;

    /// <summary>
    /// Indica si la notificación fue enviada exitosamente al Bot de Telegram.
    /// </summary>
    public bool Enviada { get; set; }

    /// <summary>
    /// Indica si el paciente leyó la notificación (según confirmación de Telegram).
    /// </summary>
    public bool Leida { get; set; }

    /// <summary>
    /// Fecha y hora en que el paciente leyó la notificación.
    /// </summary>
    public DateTime? FechaLeida { get; set; }

    // Propiedades navegacionales
    public virtual Cliente Cliente { get; set; } = null!;
    public virtual Producto? Producto { get; set; }
    public virtual Lote? Lote { get; set; }
}
