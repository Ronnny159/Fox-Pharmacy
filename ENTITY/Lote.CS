using System;

namespace Entity;

/// <summary>
/// Gestión de la inversión por adquisición: cada compra al proveedor
/// genera un lote independiente con atributos propios.
/// Unidad fundamental para el algoritmo FEFO.
/// </summary>

public class Lote : BaseEntity
{
    public int IdLote { get; set; }
    public string CodigoLote { get; set; } = string.Empty;
    public int IdProducto { get; set; }
    public DateTime FechaFabricacion { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public decimal PrecioCompra { get; set; }
    public decimal PrecioVenta { get; set; }
    public int CantidadActual { get; private set; }
    public int CantidadInicial { get; private set; }
    public char Estado { get; set; } = 'A';

    public virtual Producto Producto { get; set; } = null!;
    public virtual ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();

    public Lote(int cantidadInicial)
    {
        CantidadInicial = cantidadInicial;
        CantidadActual = cantidadInicial;
    }

    public void Descontar(int cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad a descontar debe ser positiva.", nameof(cantidad));
        if (CantidadActual < cantidad)
            throw new InvalidOperationException($"Stock insuficiente en lote {CodigoLote}. Disponible: {CantidadActual}, Solicitado: {cantidad}.");
        CantidadActual -= cantidad;
        if (CantidadActual == 0) Estado = 'B';
    }

    public void Restaurar(int cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad a restaurar debe ser positiva.", nameof(cantidad));
        CantidadActual += cantidad;
        if (Estado == 'B' && CantidadActual > 0) Estado = 'A';
    }

    public bool EstaEnVentanaCritica(int diasCriticos = 30)
    {
        return FechaVencimiento > DateTime.Now && (FechaVencimiento - DateTime.Now).TotalDays <= diasCriticos;
    }

    public bool EstaVencido => FechaVencimiento <= DateTime.Now;
    public int DiasParaVencimiento => (FechaVencimiento - DateTime.Now).Days;
    public bool EstaActivo => Estado == 'A';
    public bool EstaAgotado => Estado == 'B';
}
