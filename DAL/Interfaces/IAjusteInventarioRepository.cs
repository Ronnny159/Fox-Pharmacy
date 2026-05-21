using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces;

/// <summary>
/// Contrato para operaciones de acceso a datos de la entidad AjusteInventario.
/// </summary>
public interface IAjusteInventarioRepository
{
    void Insertar(AjusteInventario ajuste);
    IEnumerable<AjusteInventario> ObtenerPorLote(int loteId);
    IEnumerable<AjusteInventario> ObtenerPorResponsable(int usuarioId);
    IEnumerable<AjusteInventario> ObtenerPorRangoFechas(DateTime desde, DateTime hasta);
}