using System;

namespace Entity;

/// <summary>
/// Registro de auditoría para cualquier movimiento de inventario
/// no relacionado con ventas: averías, vencimientos, retiros legales.
/// </summary>
public class AjusteInventario : BaseEntity
{
    /// <summary>
    /// Identificador del lote afectado por el ajuste.
    /// </summary>
    public int LoteId { get; set; }

    /// <summary>
    /// Tipo de ajuste según la naturaleza de la merma o corrección.
    /// </summary>
    public TipoAjuste Tipo { get; set; }

    /// <summary>
    /// Cantidad de unidades ajustadas.
    /// Negativa para bajas (pérdidas), positiva para altas (correcciones).
    /// </summary>
    public int Cantidad { get; set; }

    /// <summary>
    /// Motivo textual detallado. Obligatorio para auditorías sanitarias.
    /// </summary>
    public string Motivo { get; set; } = string.Empty;

    /// <summary>
    /// Fecha en que se realizó el ajuste.
    /// </summary>
    public DateTime FechaAjuste { get; set; } = DateTime.Now;

    /// <summary>
    /// Identificador del usuario responsable del ajuste.
    /// </summary>
    public int ResponsableId { get; set; }

    // Propiedades navegacionales
    public virtual Lote Lote { get; set; } = null!;
    public virtual Usuario Responsable { get; set; } = null!;
}