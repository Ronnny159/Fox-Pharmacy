using System;
using System.Collections.Generic;

namespace Entity;

/// <summary>
/// Paciente o cliente frecuente de la farmacia.
/// Base para el programa de fidelización y notificaciones por Telegram.
/// </summary>

public class Cliente : BaseEntity
{
    public int IdCliente { get; set; }
    public string Documento { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string? Correo { get; set; }
    public string? ChatId { get; set; }
    public string? MedicamentoRecurrente { get; set; }
    public char Estado { get; set; } = 'A';

    public bool EstaActivo => Estado == 'A';
    public bool TieneTelegram => !string.IsNullOrEmpty(ChatId);

    public virtual ICollection<Venta> Compras { get; set; } = new List<Venta>();
}