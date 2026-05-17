using System;

namespace PharmaSmart.Entities
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public string CodigoBarras { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int StockMinimo { get; set; }
        public string UnidadMedida { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        // Propiedad calculada (no mapeada directamente)
        public string NombreCompleto => $"{CodigoBarras} - {Nombre}";
    }
}