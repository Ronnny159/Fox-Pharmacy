using System;
using BLL.DTOs;
using Entity;

namespace BLL.Interfaces;

/// <summary>
/// Contrato para la lógica de negocio relacionada con clientes de fidelización.
/// </summary>
public interface IClienteService
{
    ResultadoOperacion ObtenerPorDocumento(string documento);
    ResultadoOperacion ObtenerPorId(int id);
    ResultadoOperacion ObtenerTodosFidelizacion();
    ResultadoOperacion Insertar(Cliente cliente);
    ResultadoOperacion Actualizar(Cliente cliente);
    ResultadoOperacion VincularTelegram(string documento, string chatId);
}