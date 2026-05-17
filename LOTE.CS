using System;

namespace Entities
{
    public class Lote
    {
        public int LoteId { get; set; }
        public int ProductoId { get; set; }
        public string CodigoLote { get; set; }
        public DateTime FechaCompra { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int CantidadInicial { get; set; }
        public int CantidadActual { get; set; }
        public string Ubicacion { get; set; }
        public string Estado { get; set; }
        public DateTime FechaIngreso { get; set; }

        // Propiedades calculadas
        public bool EstaVencido => FechaVencimiento < DateTime.Today;
        public bool EstaPorVencer => FechaVencimiento <= DateTime.Today.AddDays(30) && !EstaVencido;
        public int DiasParaVencer => (int)(FechaVencimiento - DateTime.Today).TotalDays;
        public decimal MargenGanancia => PrecioVenta - PrecioCompra;
        public decimal PorcentajeGanancia => PrecioCompra > 0 ?
            Math.Round((MargenGanancia / PrecioCompra) * 100, 2) : 0;
    }
}