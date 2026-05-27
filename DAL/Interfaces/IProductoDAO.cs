using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces;

/// <summary>
/// Contrato para operaciones de acceso a datos de Producto.
/// Todas las operaciones delegan en procedimientos almacenados PL/SQL.
/// Principio aplicado: Interface Segregation (ISP).
/// </summary>
public interface IProductoDAO
{
    Producto? ObtenerPorCodigo(string codigo);
    Producto? ObtenerPorId(int id);
    Producto? ObtenerPorNombre(string nombre);
    List<Producto> ObtenerTodos();
    void Insertar(Producto producto);
    void Actualizar(Producto producto);
    void ActualizarDescuentoIndividual(int productoId, decimal? descuento);
    void Eliminar(int id);
}