using System;

namespace Entity;

/// <summary>
/// Representa una alerta o notificación enviada a un paciente
/// del programa de fidelización a través del Bot de Telegram.
/// </summary>

public class AlertaFidelizacion : BaseEntity
{
    public int IdAlerta { get; set; }
    public int IdCliente { get; set; }
    public int? IdProducto { get; set; }
    public int? IdLote { get; set; }
    public char TipoAlerta { get; set; } = 'P';
    public string Mensaje { get; set; } = string.Empty;
    public DateTime FechaEnvio { get; set; } = DateTime.Now;
    public char Estado { get; set; } = 'P';
    public DateTime? FechaLeida { get; set; }

    public bool Enviada => Estado == 'E' || Estado == 'L';
    public bool Leida => Estado == 'L';

    public virtual Cliente Cliente { get; set; } = null!;
    public virtual Producto? Producto { get; set; }
    public virtual Lote? Lote { get; set; }
}
