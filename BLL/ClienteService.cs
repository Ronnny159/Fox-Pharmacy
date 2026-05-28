using BLL.DTOs;
using BLL.Interfaces;
using DAL.Interfaces;
using Entity;

namespace BLL.Services;

/// <summary>
/// Lógica de negocio para la gestión de clientes del programa de fidelización.
/// Aplica validaciones de negocio antes de delegar en el repositorio.
/// </summary>
public class ClienteService : IClienteService
{
    private readonly IClienteDAO _clienteDAO;

    public ClienteService(IClienteDAO clienteDAO)
    {
        _clienteDAO = clienteDAO;
    }

    /// <summary>
    /// Busca un cliente por su número de documento de identidad.
    /// </summary>
    public ResultadoOperacion ObtenerPorDocumento(string documento)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(documento))
                return ResultadoOperacion.Fallo("El número de documento es obligatorio.");

            var cliente = _clienteDAO.ObtenerPorDocumento(documento.Trim());

            if (cliente is null)
                return ResultadoOperacion.Fallo(
                    $"No se encontró ningún cliente con el documento '{documento}'.");

            return ResultadoOperacion.Exito(
                $"Cliente '{cliente.NombreCompleto}' encontrado.",
                cliente);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Busca un cliente por su identificador interno.
    /// </summary>
    public ResultadoOperacion ObtenerPorId(int id)
    {
        try
        {
            if (id <= 0)
                return ResultadoOperacion.Fallo("El identificador del cliente no es válido.");

            var cliente = _clienteDAO.ObtenerPorId(id);

            if (cliente is null)
                return ResultadoOperacion.Fallo(
                    $"No se encontró ningún cliente con el ID {id}.");

            return ResultadoOperacion.Exito(
                $"Cliente '{cliente.NombreCompleto}' encontrado.",
                cliente);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Retorna todos los clientes inscritos en el programa de fidelización
    /// (aquellos con registro activo en el sistema).
    /// </summary>
    public ResultadoOperacion ObtenerTodosFidelizacion()
    {
        try
        {
            var clientes = _clienteDAO.ObtenerTodosFidelizacion().ToList();

            if (clientes.Count == 0)
                return ResultadoOperacion.Exito(
                    "No hay clientes registrados en el programa de fidelización.",
                    new List<Cliente>());

            var resumen = clientes.Select(c => new
            {
                c.Id,
                c.Documento,
                c.NombreCompleto,
                c.Telefono,
                Correo = c.Correo ?? "No registrado",
                TieneTelegram = c.TieneTelegram,
                MedicamentoRecurrente = c.MedicamentoRecurrente ?? "No registrado"
            }).ToList();

            return ResultadoOperacion.Exito(
                $"Se encontraron {resumen.Count} cliente(s) en el programa de fidelización.",
                resumen);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Registra un nuevo cliente en el programa de fidelización.
    /// Valida que el documento y el nombre no estén vacíos,
    /// y que el documento no esté ya registrado en el sistema.
    /// </summary>
    public ResultadoOperacion Insertar(Cliente cliente)
    {
        try
        {
            if (cliente is null)
                return ResultadoOperacion.Fallo("Los datos del cliente son obligatorios.");

            if (string.IsNullOrWhiteSpace(cliente.Documento))
                return ResultadoOperacion.Fallo("El número de documento es obligatorio.");

            if (string.IsNullOrWhiteSpace(cliente.NombreCompleto))
                return ResultadoOperacion.Fallo("El nombre completo del cliente es obligatorio.");

            if (string.IsNullOrWhiteSpace(cliente.Telefono))
                return ResultadoOperacion.Fallo("El teléfono del cliente es obligatorio.");

            // Verificar duplicado por documento
            var existente = _clienteDAO.ObtenerPorDocumento(cliente.Documento.Trim());
            if (existente is not null)
                return ResultadoOperacion.Fallo(
                    $"Ya existe un cliente registrado con el documento '{cliente.Documento}'.");

            // Normalizar datos antes de persistir
            cliente.Documento = cliente.Documento.Trim();
            cliente.NombreCompleto = cliente.NombreCompleto.Trim();
            cliente.Telefono = cliente.Telefono.Trim();
            cliente.Correo = cliente.Correo?.Trim();

            _clienteDAO.Insertar(cliente);

            return ResultadoOperacion.Exito(
                $"Cliente '{cliente.NombreCompleto}' registrado correctamente.",
                cliente);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Actualiza los datos de un cliente existente.
    /// No permite cambiar el número de documento; ese campo es inmutable.
    /// </summary>
    public ResultadoOperacion Actualizar(Cliente cliente)
    {
        try
        {
            if (cliente is null)
                return ResultadoOperacion.Fallo("Los datos del cliente son obligatorios.");

            if (cliente.Id <= 0)
                return ResultadoOperacion.Fallo("El identificador del cliente no es válido.");

            if (string.IsNullOrWhiteSpace(cliente.NombreCompleto))
                return ResultadoOperacion.Fallo("El nombre completo del cliente es obligatorio.");

            if (string.IsNullOrWhiteSpace(cliente.Telefono))
                return ResultadoOperacion.Fallo("El teléfono del cliente es obligatorio.");

            // Verificar que el cliente existe antes de actualizar
            var existente = _clienteDAO.ObtenerPorId(cliente.Id);
            if (existente is null)
                return ResultadoOperacion.Fallo(
                    $"No se encontró ningún cliente con el ID {cliente.Id}.");

            // Normalizar datos antes de persistir
            cliente.NombreCompleto = cliente.NombreCompleto.Trim();
            cliente.Telefono = cliente.Telefono.Trim();
            cliente.Correo = cliente.Correo?.Trim();

            _clienteDAO.Actualizar(cliente);

            return ResultadoOperacion.Exito(
                $"Cliente '{cliente.NombreCompleto}' actualizado correctamente.",
                cliente);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Vincula el ChatId de Telegram a un cliente identificado por su documento.
    /// Este proceso ocurre cuando el paciente ejecuta /registrar en el Bot.
    /// Verifica que el ChatId no esté ya asignado a otro cliente.
    /// </summary>
    public ResultadoOperacion VincularTelegram(string documento, string chatId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(documento))
                return ResultadoOperacion.Fallo("El número de documento es obligatorio.");

            if (string.IsNullOrWhiteSpace(chatId))
                return ResultadoOperacion.Fallo("El ChatId de Telegram es obligatorio.");

            // Verificar que el ChatId no esté ya vinculado a otro cliente
            var clienteConChatId = _clienteDAO.ObtenerPorChatId(chatId.Trim());
            if (clienteConChatId is not null)
                return ResultadoOperacion.Fallo(
                    $"El ChatId de Telegram ya está vinculado al cliente '{clienteConChatId.NombreCompleto}'. " +
                    "Cada cuenta de Telegram solo puede estar asociada a un cliente.");

            var cliente = _clienteDAO.ObtenerPorDocumento(documento.Trim());
            if (cliente is null)
                return ResultadoOperacion.Fallo(
                    $"No se encontró ningún cliente con el documento '{documento}'. " +
                    "El paciente debe estar registrado en el sistema antes de vincular Telegram.");

            if (cliente.TieneTelegram)
                return ResultadoOperacion.Fallo(
                    $"El cliente '{cliente.NombreCompleto}' ya tiene un ChatId de Telegram vinculado. " +
                    "Si desea actualizarlo, contacte al administrador.");

            cliente.ChatId = chatId.Trim();
            _clienteDAO.Actualizar(cliente);

            return ResultadoOperacion.Exito(
                $"Telegram vinculado correctamente al cliente '{cliente.NombreCompleto}'.",
                new
                {
                    cliente.Id,
                    cliente.NombreCompleto,
                    cliente.ChatId
                });
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }
}
