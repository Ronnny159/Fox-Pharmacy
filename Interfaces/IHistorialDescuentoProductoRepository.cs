using System;
using System.Collections.Generic;
using Entity;

namespace DAL.Interfaces;

/// <summary>
/// Contrato para operaciones de acceso a datos de la entidad HistorialDescuentoProducto.
/// </summary>
public interface IHistorialDescuentoProductoRepository
{
    void Insertar(HistorialDescuentoProducto historial);
    List<HistorialDescuentoProducto> ObtenerPorProducto(int productoId);
    List<HistorialDescuentoProducto> ObtenerTodos();
}