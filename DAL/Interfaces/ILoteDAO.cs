using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces;

/// <summary>
/// Contrato para operaciones de acceso a datos de Lote.
/// Incluye el método crítico FEFO que delega en SP_SELECCIONAR_LOTE_FEFO.
/// </summary>
public interface ILoteDAO
{
    Lote? ObtenerPorId(int id);
    List<Lote> ObtenerPorProducto(int productoId);
    List<Lote> ObtenerTodosActivos();
    void Insertar(Lote lote);
    void Actualizar(Lote lote);
    void ActualizarStock(int loteId, int nuevaCantidad, EstadoLote estado);

    /// <summary>
    /// Aplica el algoritmo FEFO llamando al procedimiento almacenado SP_SELECCIONAR_LOTE_FEFO.
    /// </summary>
    Lote? SeleccionarLoteFEFO(int productoId);
}