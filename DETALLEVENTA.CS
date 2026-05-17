using System;

namespace Entity;

/// <summary>
/// Registro granular de cada ítem vendido, con trazabilidad completa al lote de origen.
/// Materializa la exigencia de trazabilidad del informe técnico.
/// </summary>
public class DetalleVenta : BaseEntity
{
    /// <summary>
    /// Identificador de la venta a la que pertenece este detalle.
    /// </summary>
    public int VentaId { get; set; }

    /// <summary>
    /// Identificador del lote del que se extrajo la mercancía.
    /// Dato crítico para trazabilidad y auditorías sanitarias.
    /// </summary>
    public int LoteId { get; set; }

    /// <summary>
    /// Cantidad de unidades vendidas de ese lote específico.
    /// </summary>
    public int Cantidad { get; set; }

    /// <summary>
    /// Precio unitario aplicado en el momento de la venta.
    /// Puede diferir del precio actual del lote si hubo descuento.
    /// </summary>
    public decimal PrecioAplicado { get; set; }

    /// <summary>
    /// Descuento unitario aplicado a este ítem (por vencimiento, fidelidad, promoción).
    /// </summary>
    public decimal DescuentoUnitario { get; set; }

    /// <summary>
    /// Subtotal calculado: (PrecioAplicado - DescuentoUnitario) * Cantidad.
    /// </summary>
    public decimal Subtotal => (PrecioAplicado - DescuentoUnitario) * Cantidad;

    // Propiedades navegacionales
    public virtual Venta Venta { get; set; } = null!;
    public virtual Lote Lote { get; set; } = null!;
}