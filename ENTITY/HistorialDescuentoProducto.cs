using System;

namespace Entity;

/// <summary>
/// Auditoría específica para cambios en descuentos individuales de productos.
/// Permite rastrear quién, cuándo y por qué modificó la política de descuentos.
/// </summary>

public class HistorialDescuentoProducto : BaseEntity
{
    public int IdHistorial { get; set; }
    public int IdProducto { get; set; }
    public string CodigoProducto { get; set; } = string.Empty;
    public string NombreProducto { get; set; } = string.Empty;
    public decimal? DescuentoAnterior { get; set; }
    public decimal? DescuentoNuevo { get; set; }
    public string Accion { get; set; } = string.Empty;
    public string Motivo { get; set; } = string.Empty;
    public DateTime FechaCambio { get; set; } = DateTime.Now;
    public int IdUsuario { get; set; }
    public string? DireccionIP { get; set; }

    public virtual Usuario ModificadoPor { get; set; } = null!;
    public virtual Producto Producto { get; set; } = null!;
}