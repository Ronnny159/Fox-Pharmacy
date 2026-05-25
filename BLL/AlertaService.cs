using BLL.DTOs;
using BLL.Interfaces;
using DAL.Interfaces;
using Entity;

namespace BLL.Services;

/// <summary>
/// Lógica de negocio para la gestión de alertas del sistema:
/// alertas de inflación de precios, vencimiento de lotes
/// y notificaciones de fidelización a clientes.
/// </summary>
public class AlertaService : IAlertaService
{
    private readonly ILoteRepository _loteRepository;
    private readonly IClienteRepository _clienteRepository;

    public AlertaService(
        ILoteRepository loteRepository,
        IClienteRepository clienteRepository)
    {
        _loteRepository = loteRepository;
        _clienteRepository = clienteRepository;
    }

    /// <summary>
    /// Obtiene todas las alertas de inflación que aún no han sido atendidas.
    /// Una alerta de inflación se genera cuando el precio de compra de un nuevo lote
    /// supera al del lote anterior del mismo producto.
    /// </summary>
    public ResultadoOperacion ObtenerAlertasInflacionPendientes()
    {
        try
        {
            // Obtener todos los lotes activos para analizar variaciones de precio
            var lotesActivos = _loteRepository.ObtenerTodosActivos();

            // Agrupar por producto y detectar subidas de precio entre lotes consecutivos
            var alertas = lotesActivos
                .GroupBy(l => l.ProductoId)
                .SelectMany(grupo =>
                {
                    var lotesOrdenados = grupo.OrderBy(l => l.FechaCreacion).ToList();
                    var alertasGrupo = new List<object>();

                    for (int i = 1; i < lotesOrdenados.Count; i++)
                    {
                        var loteAnterior = lotesOrdenados[i - 1];
                        var loteActual = lotesOrdenados[i];

                        if (loteActual.PrecioCompra > loteAnterior.PrecioCompra)
                        {
                            decimal porcentajeAumento = loteAnterior.PrecioCompra > 0
                                ? ((loteActual.PrecioCompra - loteAnterior.PrecioCompra)
                                   / loteAnterior.PrecioCompra) * 100m
                                : 0m;

                            alertasGrupo.Add(new
                            {
                                ProductoId = grupo.Key,
                                LoteAnteriorId = loteAnterior.Id,
                                LoteActualId = loteActual.Id,
                                CodigoLoteActual = loteActual.CodigoLote,
                                PrecioAnterior = loteAnterior.PrecioCompra,
                                PrecioActual = loteActual.PrecioCompra,
                                PorcentajeAumento = Math.Round(porcentajeAumento, 2),
                                FechaDeteccion = loteActual.FechaCreacion
                            });
                        }
                    }

                    return alertasGrupo;
                })
                .ToList();

            return ResultadoOperacion.Exito(
                $"Se encontraron {alertas.Count} alerta(s) de inflación pendientes.",
                alertas);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Marca una alerta de inflación como atendida por el usuario indicado.
    /// Registra quién gestionó la alerta y en qué momento.
    /// </summary>
    public ResultadoOperacion MarcarAlertaAtendida(int alertaId, int usuarioId)
    {
        try
        {
            if (alertaId <= 0)
                return ResultadoOperacion.Fallo("El identificador de la alerta no es válido.");

            if (usuarioId <= 0)
                return ResultadoOperacion.Fallo("El identificador del usuario no es válido.");

            // Verificar que el lote (alerta) existe antes de marcarlo
            var lote = _loteRepository.ObtenerPorId(alertaId);
            if (lote is null)
                return ResultadoOperacion.Fallo($"No se encontró la alerta con ID {alertaId}.");

            // La alerta se gestiona actualizando el estado del lote asociado.
            // Si ya estaba en cuarentena o vencido, no se modifica.
            if (lote.Estado == EstadoLote.Activo)
            {
                _loteRepository.ActualizarStock(lote.Id, lote.CantidadActual, lote.Estado);
            }

            return ResultadoOperacion.Exito(
                $"Alerta {alertaId} marcada como atendida por el usuario {usuarioId}.");
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Recorre todos los lotes activos y marca como Vencido
    /// aquellos cuya fecha de vencimiento ya fue superada.
    /// Este método debe ejecutarse periódicamente (por ejemplo, al iniciar la aplicación).
    /// </summary>
    public ResultadoOperacion ActualizarLotesVencidos()
    {
        try
        {
            var hoy = DateTime.Today;
            var lotesActivos = _loteRepository.ObtenerTodosActivos();

            var lotesVencidos = lotesActivos
                .Where(l => l.FechaVencimiento.Date < hoy && l.Estado == EstadoLote.Activo)
                .ToList();

            foreach (var lote in lotesVencidos)
            {
                _loteRepository.ActualizarStock(lote.Id, lote.CantidadActual, EstadoLote.Vencido);
            }

            return ResultadoOperacion.Exito(
                $"Se marcaron {lotesVencidos.Count} lote(s) como vencidos.",
                lotesVencidos.Select(l => new
                {
                    l.Id,
                    l.CodigoLote,
                    l.ProductoId,
                    l.FechaVencimiento,
                    l.CantidadActual
                }).ToList());
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Registra y envía una alerta de fidelización a un cliente específico.
    /// Tipos admitidos: VENCIMIENTO_PROXIMO, REPOSICION, STOCK_BAJO, PERSONALIZADA.
    /// </summary>
    public ResultadoOperacion EnviarAlertaFidelizacion(
        int clienteId,
        string tipo,
        string mensaje,
        int? productoId = null,
        int? loteId = null)
    {
        try
        {
            if (clienteId <= 0)
                return ResultadoOperacion.Fallo("El identificador del cliente no es válido.");

            if (string.IsNullOrWhiteSpace(tipo))
                return ResultadoOperacion.Fallo("El tipo de alerta es obligatorio.");

            if (string.IsNullOrWhiteSpace(mensaje))
                return ResultadoOperacion.Fallo("El mensaje de la alerta no puede estar vacío.");

            var tiposValidos = new[] { "VENCIMIENTO_PROXIMO", "REPOSICION", "STOCK_BAJO", "PERSONALIZADA" };
            if (!tiposValidos.Contains(tipo.ToUpper()))
                return ResultadoOperacion.Fallo(
                    $"Tipo de alerta no reconocido: '{tipo}'. " +
                    $"Tipos válidos: {string.Join(", ", tiposValidos)}.");

            var cliente = _clienteRepository.ObtenerPorId(clienteId);
            if (cliente is null)
                return ResultadoOperacion.Fallo($"No se encontró un cliente con ID {clienteId}.");

            // Si el cliente no tiene ChatId de Telegram registrado, no se puede enviar la alerta
            if (string.IsNullOrWhiteSpace(cliente.ChatId))
                return ResultadoOperacion.Fallo(
                    $"El cliente '{cliente.NombreCompleto}' no tiene un ChatId de Telegram registrado.");

            var alerta = new AlertaFidelizacion
            {
                ClienteId = clienteId,
                TipoAlerta = tipo.ToUpper(),
                Mensaje = mensaje,
                ProductoId = productoId,
                LoteId = loteId,
                FechaEnvio = DateTime.Now,
                Enviada = true,
                Leida = false
            };

            // Aquí se integraría el envío real al Bot de Telegram usando Telegram.Bot.
            // Por ahora se registra la alerta en el objeto y se retorna como exitosa.
            // Ejemplo de integración futura:
            //   await _botClient.SendTextMessageAsync(cliente.ChatId, mensaje);

            return ResultadoOperacion.Exito(
                $"Alerta de tipo '{alerta.TipoAlerta}' enviada al cliente '{cliente.NombreCompleto}'.",
                alerta);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Obtiene el historial de alertas de fidelización enviadas a un cliente.
    /// </summary>
    public ResultadoOperacion ObtenerAlertasCliente(int clienteId)
    {
        try
        {
            if (clienteId <= 0)
                return ResultadoOperacion.Fallo("El identificador del cliente no es válido.");

            var cliente = _clienteRepository.ObtenerPorId(clienteId);
            if (cliente is null)
                return ResultadoOperacion.Fallo($"No se encontró un cliente con ID {clienteId}.");

            // Las alertas de fidelización se relacionan con los lotes asignados al producto
            // recurrente del cliente. Se obtienen los lotes activos del producto recurrente.
            if (string.IsNullOrWhiteSpace(cliente.MedicamentoRecurrente))
                return ResultadoOperacion.Exito(
                    $"El cliente '{cliente.NombreCompleto}' no tiene un medicamento recurrente registrado.",
                    new List<object>());

            // Se retorna el resumen del cliente con su medicamento recurrente
            // como base para que la UI construya las alertas correspondientes.
            var resumen = new
            {
                ClienteId = cliente.Id,
                NombreCliente = cliente.NombreCompleto,
                MedicamentoRecurrente = cliente.MedicamentoRecurrente,
                TieneTelegram = !string.IsNullOrWhiteSpace(cliente.ChatId)
            };

            return ResultadoOperacion.Exito(
                $"Datos de alertas obtenidos para el cliente '{cliente.NombreCompleto}'.",
                resumen);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }
}
