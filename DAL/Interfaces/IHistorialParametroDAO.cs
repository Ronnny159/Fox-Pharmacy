using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Interfaces;

/// <summary>
/// Contrato para operaciones de acceso a datos de la entidad HistorialParametro.
/// </summary>
public interface IHistorialParametroDAO
{
    void Insertar(HistorialParametro historial);
    List<HistorialParametro> ObtenerPorParametro(string clave);
    List<HistorialParametro> ObtenerPorUsuario(int usuarioId);
    List<HistorialParametro> ObtenerPorRangoFechas(DateTime desde, DateTime hasta);
}