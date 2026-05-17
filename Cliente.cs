using System;
using System.Collections.Generic;

namespace Entity;

/// <summary>
/// Paciente o cliente frecuente de la farmacia.
/// Base para el programa de fidelización y notificaciones por Telegram.
/// </summary>
public class Cliente : BaseEntity
{
    /// <summary>
    /// Número de documento de identidad del paciente.
    /// </summary>
    public string Documento { get; set; } = string.Empty;

    /// <summary>
    /// Nombre completo del paciente.
    /// </summary>
    public string NombreCompleto { get; set; } = string.Empty;

    /// <summary>
    /// Número de teléfono para contacto y notificaciones.
    /// </summary>
    public string Telefono { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico para comunicaciones.
    /// </summary>
    public string? Correo { get; set; }

    /// <summary>
    /// Identificador de chat de Telegram para el BotService.
    /// Se obtiene cuando el paciente se registra mediante el comando /registrar.
    /// </summary>
    public string? ChatId { get; set; }

    /// <summary>
    /// Medicamento de tratamiento crónico asociado al paciente.
    /// Base para notificaciones de reposición y alertas de vencimiento.
    /// </summary>
    public string? MedicamentoRecurrente { get; set; }

    /// <summary>
    /// Indica si el paciente está registrado en el Bot de Telegram.
    /// </summary>
    public bool TieneTelegram => !string.IsNullOrEmpty(ChatId);

    // Propiedades navegacionales
    public virtual ICollection<Venta> Compras { get; set; } = new List<Venta>();
}