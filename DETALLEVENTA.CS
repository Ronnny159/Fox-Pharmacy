using System;

namespace Entities
{
    public class DetalleVenta
    {
        public int DetalleId { get; set; }
        public int VentaId { get; set; }
        public int LoteId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal DescuentoAplicado { get; set; }

        // Navegación
        public Lote Lote { get; set; }
        public Venta Venta { get; set; }

        // Propiedades calculadas
        public decimal Subtotal => Cantidad * PrecioUnitario;
        public decimal SubtotalConDescuento => Subtotal * (1 - DescuentoAplicado / 100);
    }
}