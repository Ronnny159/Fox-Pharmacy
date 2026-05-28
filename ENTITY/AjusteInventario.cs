using System;

namespace Entity;

/// <summary>
/// Registro de auditoría para cualquier movimiento de inventario
/// no relacionado con ventas: averías, vencimientos, retiros legales.
/// </summary>

public class AjusteInventario : BaseEntity
{
    public int IdAjuste { get; set; }
    public int IdLote { get; set; }
    public TipoAjuste Tipo { get; set; }
    public int Cantidad { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public DateTime FechaAjuste { get; set; } = DateTime.Now;
    public int IdResponsable { get; set; }

    public virtual Lote Lote { get; set; } = null!;
    public virtual Usuario Responsable { get; set; } = null!;
}