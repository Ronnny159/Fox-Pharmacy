using System;
using System.Collections.Generic;
using Entity;

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