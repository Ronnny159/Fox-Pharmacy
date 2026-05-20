using System;

namespace Entity;

/// <summary>
/// Gestión de la inversión por adquisición: cada compra al proveedor
/// genera un lote independiente con atributos propios.
/// Unidad fundamental para el algoritmo FEFO.
/// </summary>
public class Lote : BaseEntity
{
    /// <summary>
    /// Código identificador humano del lote (número de factura del proveedor, etc.).
    /// </summary>
    public string CodigoLote { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del producto al que pertenece este lote.
    /// </summary>
    public int ProductoId { get; set; }

    /// <summary>
    /// Fecha de fabricación del lote.
    /// </summary>
    public DateTime FechaFabricacion { get; set; }

    /// <summary>
    /// Fecha de vencimiento del lote. Dato crítico para el algoritmo FEFO.
    /// </summary>
    public DateTime FechaVencimiento { get; set; }

    /// <summary>
    /// Precio de compra unitario pagado al proveedor. Base para control de márgenes.
    /// </summary>
    public decimal PrecioCompra { get; set; }

    /// <summary>
    /// Precio de venta al público fijado para este lote específico.
    /// </summary>
    public decimal PrecioVenta { get; set; }

    /// <summary>
    /// Unidades disponibles actualmente para la venta.
    /// </summary>
    public int CantidadActual { get; private set; }

    /// <summary>
    /// Cantidad inicial con la que ingresó el lote. Sirve para conciliaciones.
    /// </summary>
    public int CantidadInicial { get; private set; }

    /// <summary>
    /// Estado operativo del lote (activo, agotado, vencido, en cuarentena).
    /// </summary>
    public EstadoLote Estado { get; set; } = EstadoLote.Activo;

    // Propiedades navegacionales
    public virtual Producto Producto { get; set; } = null!;
    public virtual ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();

    /// <summary>
    /// Constructor seguro que inicializa las cantidades.
    /// </summary>
    /// <param name="cantidadInicial">Unidades recibidas del proveedor.</param>
    public Lote(int cantidadInicial)
    {
        CantidadInicial = cantidadInicial;
        CantidadActual = cantidadInicial;
    }

    /// <summary>
    /// Descuenta unidades del lote por una venta.
    /// Lanza excepción de dominio si la cantidad es inválida o insuficiente.
    /// </summary>
    /// <param name="cantidad">Unidades a descontar.</param>
    /// <exception cref="ArgumentException">Si la cantidad no es positiva.</exception>
    /// <exception cref="InvalidOperationException">Si no hay stock suficiente.</exception>
    public void Descontar(int cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad a descontar debe ser positiva.", nameof(cantidad));

        if (CantidadActual < cantidad)
            throw new InvalidOperationException(
                $"Stock insuficiente en lote {CodigoLote}. Disponible: {CantidadActual}, Solicitado: {cantidad}.");

        CantidadActual -= cantidad;

        if (CantidadActual == 0)
            Estado = EstadoLote.Agotado;
    }

    /// <summary>
    /// Restaura unidades al lote (anulación de venta o ajuste positivo).
    /// </summary>
    /// <param name="cantidad">Unidades a restaurar.</param>
    /// <exception cref="ArgumentException">Si la cantidad no es positiva.</exception>
    public void Restaurar(int cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad a restaurar debe ser positiva.", nameof(cantidad));

        CantidadActual += cantidad;

        if (Estado == EstadoLote.Agotado && CantidadActual > 0)
            Estado = EstadoLote.Activo;
    }

    /// <summary>
    /// Indica si el lote está en ventana crítica de vencimiento (30 días o menos).
    /// </summary>
    public bool EstaEnVentanaCritica(int diasCriticos = 30)
    {
        return FechaVencimiento > DateTime.Now &&
               (FechaVencimiento - DateTime.Now).TotalDays <= diasCriticos;
    }

    /// <summary>
    /// Indica si el lote ya venció.
    /// </summary>
    public bool EstaVencido => FechaVencimiento <= DateTime.Now;

    /// <summary>
    /// Días restantes hasta la fecha de vencimiento.
    /// </summary>
    public int DiasParaVencimiento => (FechaVencimiento - DateTime.Now).Days;
}
