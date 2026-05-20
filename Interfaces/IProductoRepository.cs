using System;
using System.Collections.Generic;
using Entity;

namespace DAL.Interfaces;

/// <summary>
/// Contrato para operaciones de acceso a datos de la entidad Producto.
/// Principio aplicado: Interface Segregation (ISP).
/// </summary>
public interface IProductoRepository
{
    Producto? ObtenerPorCodigo(string codigo);
    Producto? ObtenerPorId(int id);
    Producto? ObtenerPorNombre(string nombre);
    IEnumerable<Producto> ObtenerTodos();
    void Insertar(Producto producto);
    void Actualizar(Producto producto);
    void ActualizarDescuentoIndividual(int productoId, decimal? descuento);
    void Eliminar(int id);
}