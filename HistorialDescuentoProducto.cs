using System;

namespace Entity;

/// <summary>
/// Auditoría específica para cambios en descuentos individuales de productos.
/// Permite rastrear quién, cuándo y por qué modificó la política de descuentos.
/// </summary>
public class HistorialDescuentoProducto : BaseEntity
{
    /// <summary>
    /// Identificador del producto cuyo descuento fue modificado.
    /// </summary>
    public int ProductoId { get; set; }

    /// <summary>
    /// Código del producto al momento del cambio (para trazabilidad histórica).
    /// </summary>
    public string CodigoProducto { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del producto al momento del cambio.
    /// </summary>
    public string NombreProducto { get; set; } = string.Empty;

    /// <summary>
    /// Descuento anterior. NULL si el producto usaba el descuento general.
    /// </summary>
    public decimal? DescuentoAnterior { get; set; }

    /// <summary>
    /// Nuevo descuento asignado. NULL si se volvió al descuento general.
    /// </summary>
    public decimal? DescuentoNuevo { get; set; }

    /// <summary>
    /// Acción realizada: "CREAR", "MODIFICAR" o "ELIMINAR" descuento individual.
    /// </summary>
    public string Accion { get; set; } = string.Empty;

    /// <summary>
    /// Motivo del cambio. Obligatorio para auditoría.
    /// </summary>
    public string Motivo { get; set; } = string.Empty;

    /// <summary>
    /// Fecha y hora exacta del cambio.
    /// </summary>
    public DateTime FechaCambio { get; set; } = DateTime.Now;

    /// <summary>
    /// Identificador del usuario administrador que realizó el cambio.
    /// </summary>
    public int ModificadoPorId { get; set; }

    /// <summary>
    /// Dirección IP desde la que se realizó el cambio.
    /// </summary>
    public string? DireccionIP { get; set; }

    // Propiedades navegacionales
    public virtual Usuario ModificadoPor { get; set; } = null!;
    public virtual Producto Producto { get; set; } = null!;
}