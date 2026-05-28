using System;
using System.Collections.Generic;

namespace Entity;

/// <summary>
/// Encabezado de cada transacción comercial (factura de venta).
/// Contiene los totales y metadatos de la operación.
/// </summary>

public class Venta : BaseEntity
{
    public int IdVenta { get; set; }
    public string NumeroFactura { get; set; } = string.Empty;
    public DateTime FechaVenta { get; set; } = DateTime.Now;
    public int IdUsuario { get; set; }
    public int? IdCliente { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DescuentoTotal { get; set; }
    public decimal Total { get; set; }
    public char Estado { get; set; } = 'A';
    public DateTime? FechaAnulacion { get; set; }
    public int? AnuladaPor { get; set; }

    public bool EstaAnulada => Estado == 'N';
    public bool EstaActiva => Estado == 'A';

    public virtual Usuario Usuario { get; set; } = null!;
    public virtual Cliente? Cliente { get; set; }
    public virtual ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
}