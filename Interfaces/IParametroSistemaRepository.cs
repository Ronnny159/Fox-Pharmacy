using System;
using System.Collections.Generic;
using Entity;

namespace DAL.Interfaces;

/// <summary>
/// Contrato para operaciones de acceso a datos de la entidad ParametroSistema.
/// </summary>
public interface IParametroSistemaRepository
{
    ParametroSistema? ObtenerPorClave(string clave);
    IEnumerable<ParametroSistema> ObtenerTodos();
    void Insertar(ParametroSistema parametro);
    void Actualizar(ParametroSistema parametro);
}