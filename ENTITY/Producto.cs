using System;
using System.Collections.Generic;

namespace Entity;

/// <summary>
/// Información legal y estática del fármaco, independiente del lote.
/// Representa el catálogo maestro de productos de la farmacia.
/// </summary>
public class Producto : BaseEntity
{
    public int IdProducto { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public char EsControlado { get; set; } = 'N';
    public int StockMinimo { get; set; }
    public decimal? DescuentoProximidadVencimiento { get; set; }
    public char Estado { get; set; } = 'A';

    public bool EsControladoBool => EsControlado == 'S';
    public bool EstaActivo => Estado == 'A';
    public bool UsaDescuentoPersonalizado => DescuentoProximidadVencimiento.HasValue;

    public string EtiquetaDescuento => UsaDescuentoPersonalizado
        ? $"Personalizado ({DescuentoProximidadVencimiento:P0})"
        : "General";

    public virtual ICollection<Lote> Lotes { get; set; } = new List<Lote>();
}
