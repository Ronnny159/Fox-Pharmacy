using System;

namespace Entity;

/// <summary>
/// Registro granular de cada ítem vendido, con trazabilidad completa al lote de origen.
/// Materializa la exigencia de trazabilidad del informe técnico.
/// </summary>

public class DetalleVenta : BaseEntity
{
    public int IdDetalle { get; set; }
    public int IdVenta { get; set; }
    public int IdProducto { get; set; }
    public int IdLote { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioAplicado { get; set; }
    public decimal DescuentoUnitario { get; set; }

    public decimal Subtotal => (PrecioAplicado - DescuentoUnitario) * Cantidad;

    public virtual Venta Venta { get; set; } = null!;
    public virtual Producto Producto { get; set; } = null!;
    public virtual Lote Lote { get; set; } = null!;
}