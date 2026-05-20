using System;
using System.Collections.Generic;

namespace Entity;

/// <summary>
/// Información legal y estática del fármaco, independiente del lote.
/// Representa el catálogo maestro de productos de la farmacia.
/// </summary>
public class Producto : BaseEntity
{
    /// <summary>
    /// Código único del producto (registro sanitario o SKU interno).
    /// </summary>
    public string Codigo { get; set; } = string.Empty;

    /// <summary>
    /// Nombre comercial del medicamento.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Descripción detallada: principio activo, presentación, concentración.
    /// </summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>
    /// Indica si el medicamento está sujeto a control especial
    /// (narcóticos, psicotrópicos, estupefacientes).
    /// </summary>
    public bool EsControlado { get; set; }

    /// <summary>
    /// Nivel mínimo de stock deseado para generación de alertas de reposición.
    /// </summary>
    public int StockMinimo { get; set; }

    /// <summary>
    /// Porcentaje de descuento individual para este producto cuando está próximo a vencer.
    /// NULL = usar el descuento general del sistema.
    /// Valor entre 0.00 (0%) y 1.00 (100%).
    /// </summary>
    public decimal? DescuentoProximidadVencimiento { get; set; }

    /// <summary>
    /// Indica si este producto tiene configurado un descuento personalizado.
    /// </summary>
    public bool UsaDescuentoPersonalizado => DescuentoProximidadVencimiento.HasValue;

    /// <summary>
    /// Etiqueta descriptiva del tipo de descuento para mostrar en UI.
    /// </summary>
    public string EtiquetaDescuento => UsaDescuentoPersonalizado
        ? $"Personalizado ({DescuentoProximidadVencimiento:P0})"
        : "General";

    // Propiedades navegacionales
    public virtual ICollection<Lote> Lotes { get; set; } = new List<Lote>();
}
