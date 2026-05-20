using System;
using System.Collections.Generic;
using Entity;

namespace DAL.Interfaces;

/// <summary>
/// Contrato para operaciones de acceso a datos de la entidad Cliente.
/// </summary>
public interface IClienteRepository
{
    Cliente? ObtenerPorDocumento(string documento);
    Cliente? ObtenerPorId(int id);
    Cliente? ObtenerPorChatId(string chatId);
    IEnumerable<Cliente> ObtenerTodosFidelizacion();
    void Insertar(Cliente cliente);
    void Actualizar(Cliente cliente);
}