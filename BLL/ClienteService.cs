using BLL.DTOs;
using BLL.Interfaces;
using DAL.Interfaces;
using Entity;

namespace BLL.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteDAO _clienteDAO;

    public ClienteService(IClienteDAO clienteDAO) { _clienteDAO = clienteDAO; }

    public ResultadoOperacion ObtenerPorDocumento(string documento)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(documento)) return ResultadoOperacion.Fallo("Documento obligatorio.");
            var cliente = _clienteDAO.ObtenerPorDocumento(documento.Trim());
            if (cliente is null) return ResultadoOperacion.Fallo($"Cliente con documento '{documento}' no encontrado.");
            return ResultadoOperacion.Exito($"Cliente '{cliente.NombreCompleto}' encontrado.", cliente);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ObtenerPorId(int id)
    {
        try
        {
            if (id <= 0) return ResultadoOperacion.Fallo("ID no válido.");
            var cliente = _clienteDAO.ObtenerPorId(id);
            if (cliente is null) return ResultadoOperacion.Fallo($"Cliente con ID {id} no encontrado.");
            return ResultadoOperacion.Exito($"Cliente '{cliente.NombreCompleto}' encontrado.", cliente);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ObtenerTodosFidelizacion()
    {
        try
        {
            var clientes = _clienteDAO.ObtenerTodosFidelizacion();
            if (!clientes.Any() ) return ResultadoOperacion.Exito("No hay clientes en fidelización.", new List<object>());
            var resumen = clientes.Select(c => new { c.IdCliente, c.Documento, c.NombreCompleto, c.Telefono, Correo = c.Correo ?? "No registrado", TieneTelegram = c.TieneTelegram, MedicamentoRecurrente = c.MedicamentoRecurrente ?? "No registrado" }).ToList();
            return ResultadoOperacion.Exito($"{resumen.Count} cliente(s) en fidelización.", resumen);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion Insertar(Cliente cliente)
    {
        try
        {
            if (cliente is null) return ResultadoOperacion.Fallo("Datos obligatorios.");
            if (string.IsNullOrWhiteSpace(cliente.Documento)) return ResultadoOperacion.Fallo("Documento obligatorio.");
            if (string.IsNullOrWhiteSpace(cliente.NombreCompleto)) return ResultadoOperacion.Fallo("Nombre obligatorio.");
            if (string.IsNullOrWhiteSpace(cliente.Telefono)) return ResultadoOperacion.Fallo("Teléfono obligatorio.");

            var existente = _clienteDAO.ObtenerPorDocumento(cliente.Documento.Trim());
            if (existente is not null) return ResultadoOperacion.Fallo($"Ya existe un cliente con documento '{cliente.Documento}'.");

            cliente.Documento = cliente.Documento.Trim();
            cliente.NombreCompleto = cliente.NombreCompleto.Trim();
            cliente.Telefono = cliente.Telefono.Trim();
            cliente.Correo = cliente.Correo?.Trim();
            _clienteDAO.Insertar(cliente);
            return ResultadoOperacion.Exito($"Cliente '{cliente.NombreCompleto}' registrado.", cliente);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion Actualizar(Cliente cliente)
    {
        try
        {
            if (cliente is null) return ResultadoOperacion.Fallo("Datos obligatorios.");
            if (cliente.IdCliente <= 0) return ResultadoOperacion.Fallo("ID no válido.");
            if (string.IsNullOrWhiteSpace(cliente.NombreCompleto)) return ResultadoOperacion.Fallo("Nombre obligatorio.");
            if (string.IsNullOrWhiteSpace(cliente.Telefono)) return ResultadoOperacion.Fallo("Teléfono obligatorio.");

            var existente = _clienteDAO.ObtenerPorId(cliente.IdCliente);
            if (existente is null) return ResultadoOperacion.Fallo($"Cliente con ID {cliente.IdCliente} no encontrado.");

            cliente.NombreCompleto = cliente.NombreCompleto.Trim();
            cliente.Telefono = cliente.Telefono.Trim();
            cliente.Correo = cliente.Correo?.Trim();
            _clienteDAO.Actualizar(cliente);
            return ResultadoOperacion.Exito($"Cliente '{cliente.NombreCompleto}' actualizado.", cliente);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion VincularTelegram(string documento, string chatId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(documento)) return ResultadoOperacion.Fallo("Documento obligatorio.");
            if (string.IsNullOrWhiteSpace(chatId)) return ResultadoOperacion.Fallo("ChatId obligatorio.");

            var clienteConChat = _clienteDAO.ObtenerPorChatId(chatId.Trim());
            if (clienteConChat is not null) return ResultadoOperacion.Fallo($"ChatId ya vinculado a '{clienteConChat.NombreCompleto}'.");

            var cliente = _clienteDAO.ObtenerPorDocumento(documento.Trim());
            if (cliente is null) return ResultadoOperacion.Fallo($"Cliente con documento '{documento}' no encontrado.");
            if (cliente.TieneTelegram) return ResultadoOperacion.Fallo($"'{cliente.NombreCompleto}' ya tiene Telegram vinculado.");

            cliente.ChatId = chatId.Trim();
            _clienteDAO.Actualizar(cliente);
            return ResultadoOperacion.Exito($"Telegram vinculado a '{cliente.NombreCompleto}'.", new { cliente.IdCliente, cliente.NombreCompleto, cliente.ChatId });
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }
}