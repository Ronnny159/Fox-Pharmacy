using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Interfaces;

/// <summary>
/// Contrato para operaciones de acceso a datos de la entidad HistorialDescuentoProducto.
/// </summary>
public interface IHistorialDescuentoProductoDAO
{
    void Insertar(HistorialDescuentoProducto historial);
    List<HistorialDescuentoProducto> ObtenerPorProducto(int productoId);
    List<HistorialDescuentoProducto> ObtenerTodos();
}