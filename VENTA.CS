using System;
using System.Collections.Generic;

namespace Entities
{
    public class Venta
    {
        public int VentaId { get; set; }
        public DateTime FechaVenta { get; set; }
        public int UsuarioId { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteDocumento { get; set; }
        public decimal TotalVenta { get; set; }
        public string FormaPago { get; set; }
        public string Estado { get; set; }

        // Navegación (se llenará desde BLL)
        public List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
        public Usuario Vendedor { get; set; }

        // Propiedades calculadas
        public int CantidadItems => Detalles?.Count ?? 0;
        public string FechaFormateada => FechaVenta.ToString("dd/MM/yyyy HH:mm");
    }
}
