using System;
using System.Collections.Generic;

namespace Entity;

/// <summary>
/// Encabezado de cada transacción comercial (factura de venta).
/// Contiene los totales y metadatos de la operación.
/// </summary>
public class Venta : BaseEntity
{
    /// <summary>
    /// Número consecutivo de factura generado automáticamente por el sistema.
    /// Formato: FAC-YYYYMMDD-SEQ (ej: FAC-20260415-01001)
    /// </summary>
    public string NumeroFactura { get; set; } = string.Empty;

    /// <summary>
    /// Fecha y hora exacta de la transacción.
    /// </summary>
    public DateTime FechaVenta { get; set; } = DateTime.Now;

    /// <summary>
    /// Identificador del usuario (cajero/farmacéutico) que realizó la venta.
    /// </summary>
    public int UsuarioId { get; set; }

    /// <summary>
    /// Identificador del cliente si la venta se asocia a un paciente de fidelización.
    /// NULL para ventas a público general.
    /// </summary>
    public int? ClienteId { get; set; }

    /// <summary>
    /// Total bruto antes de descuentos (suma de precios de venta originales).
    /// </summary>
    public decimal Subtotal { get; set; }

    /// <summary>
    /// Monto total de descuentos aplicados (proximidad de vencimiento, fidelización, etc.).
    /// </summary>
    public decimal DescuentoTotal { get; set; }

    /// <summary>
    /// Total neto a pagar por el cliente (Subtotal - DescuentoTotal).
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Indica si la venta fue anulada posteriormente.
    /// </summary>
    public bool Anulada { get; set; }

    /// <summary>
    /// Fecha y hora de la anulación, si aplica.
    /// </summary>
    public DateTime? FechaAnulacion { get; set; }

    /// <summary>
    /// Usuario que anuló la venta, si aplica.
    /// </summary>
    public int? AnuladaPor { get; set; }

    // Propiedades navegacionales
    public virtual Usuario Usuario { get; set; } = null!;
    public virtual Cliente? Cliente { get; set; }
    public virtual ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
}