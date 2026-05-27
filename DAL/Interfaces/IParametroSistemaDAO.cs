using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Interfaces;

/// <summary>
/// Contrato para operaciones de acceso a datos de la entidad ParametroSistema.
/// </summary>
public interface IParametroSistemaDAO
{
    ParametroSistema? ObtenerPorClave(string clave);
    IEnumerable<ParametroSistema> ObtenerTodos();
    void Insertar(ParametroSistema parametro);
    void Actualizar(ParametroSistema parametro);
}