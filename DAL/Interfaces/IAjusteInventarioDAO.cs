using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces;

/// <summary>
/// Contrato para operaciones de acceso a datos de AjusteInventario.
/// La inserción delega en SP_REGISTRAR_AJUSTE_INVENTARIO.
/// </summary>
public interface IAjusteInventarioDAO
{
    void Insertar(AjusteInventario ajuste);
    List<AjusteInventario> ObtenerPorLote(int loteId);
    List<AjusteInventario> ObtenerPorResponsable(int usuarioId);
    List<AjusteInventario> ObtenerPorRangoFechas(DateTime desde, DateTime hasta);
}