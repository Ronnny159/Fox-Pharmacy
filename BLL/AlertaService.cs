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
    private readonly ILoteDAO _loteDAO;
    private readonly IClienteDAO _clienteDAO;

    public AlertaService(ILoteDAO loteDAO, IClienteDAO clienteDAO) { _loteDAO = loteDAO; _clienteDAO = clienteDAO; }

    public ResultadoOperacion ObtenerAlertasInflacionPendientes()
    {
        try
        {
            var lotes = _loteDAO.ObtenerTodosActivos();
            var alertas = lotes.GroupBy(l => l.IdProducto).SelectMany(grupo =>
            {
                var ordenados = grupo.OrderBy(l => l.FechaCreacion).ToList();
                var lista = new List<object>();
                for (int i = 1; i < ordenados.Count; i++)
                {
                    var ant = ordenados[i - 1]; var act = ordenados[i];
                    if (act.PrecioCompra > ant.PrecioCompra)
                    {
                        decimal pct = ant.PrecioCompra > 0 ? ((act.PrecioCompra - ant.PrecioCompra) / ant.PrecioCompra) * 100m : 0m;
                        lista.Add(new { ProductoId = grupo.Key, LoteAnteriorId = ant.IdLote, LoteActualId = act.IdLote, CodigoLoteActual = act.CodigoLote, PrecioAnterior = ant.PrecioCompra, PrecioActual = act.PrecioCompra, PorcentajeAumento = Math.Round(pct, 2), FechaDeteccion = act.FechaCreacion });
                    }
                }
                return lista;
            }).ToList();
            return ResultadoOperacion.Exito($"{alertas.Count} alerta(s) de inflación.", alertas);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion MarcarAlertaAtendida(int alertaId, int usuarioId)
    {
        try
        {
            if (alertaId <= 0) return ResultadoOperacion.Fallo("ID de alerta no válido.");
            if (usuarioId <= 0) return ResultadoOperacion.Fallo("ID de usuario no válido.");
            var lote = _loteDAO.ObtenerPorId(alertaId);
            if (lote is null) return ResultadoOperacion.Fallo($"Alerta con ID {alertaId} no encontrada.");
            if (lote.Estado == 'A') _loteDAO.ActualizarStock(lote.IdLote, lote.CantidadActual, EstadoLote.Activo);
            return ResultadoOperacion.Exito($"Alerta {alertaId} atendida por usuario {usuarioId}.");
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ActualizarLotesVencidos()
    {
        try
        {
            var hoy = DateTime.Today;
            var vencidos = _loteDAO.ObtenerTodosActivos().Where(l => l.FechaVencimiento.Date < hoy && l.Estado == 'A').ToList();
            foreach (var l in vencidos) _loteDAO.ActualizarStock(l.IdLote, l.CantidadActual, EstadoLote.Vencido);
            return ResultadoOperacion.Exito($"{vencidos.Count} lote(s) marcados como vencidos.", vencidos.Select(l => new { l.IdLote, l.CodigoLote, l.IdProducto, l.FechaVencimiento, l.CantidadActual }).ToList());
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion EnviarAlertaFidelizacion(int clienteId, string tipo, string mensaje, int? productoId = null, int? loteId = null)
    {
        try
        {
            if (clienteId <= 0) return ResultadoOperacion.Fallo("ID de cliente no válido.");
            if (string.IsNullOrWhiteSpace(tipo)) return ResultadoOperacion.Fallo("Tipo obligatorio.");
            if (string.IsNullOrWhiteSpace(mensaje)) return ResultadoOperacion.Fallo("Mensaje obligatorio.");
            var tiposValidos = new[] { "VENCIMIENTO_PROXIMO", "REPOSICION", "STOCK_BAJO", "PERSONALIZADA" };
            if (!tiposValidos.Contains(tipo.ToUpper())) return ResultadoOperacion.Fallo($"Tipo no reconocido: '{tipo}'.");

            var cliente = _clienteDAO.ObtenerPorId(clienteId);
            if (cliente is null) return ResultadoOperacion.Fallo($"Cliente con ID {clienteId} no encontrado.");
            if (string.IsNullOrWhiteSpace(cliente.ChatId)) return ResultadoOperacion.Fallo($"'{cliente.NombreCompleto}' no tiene Telegram vinculado.");

            var alerta = new AlertaFidelizacion { IdCliente = clienteId, TipoAlerta = tipo.ToUpper()[0], Mensaje = mensaje, IdProducto = productoId, IdLote = loteId, FechaEnvio = DateTime.Now, Estado = 'E' };
            return ResultadoOperacion.Exito($"Alerta enviada a '{cliente.NombreCompleto}'.", alerta);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ObtenerAlertasCliente(int clienteId)
    {
        try
        {
            if (clienteId <= 0) return ResultadoOperacion.Fallo("ID de cliente no válido.");
            var cliente = _clienteDAO.ObtenerPorId(clienteId);
            if (cliente is null) return ResultadoOperacion.Fallo($"Cliente con ID {clienteId} no encontrado.");
            if (string.IsNullOrWhiteSpace(cliente.MedicamentoRecurrente)) return ResultadoOperacion.Exito($"'{cliente.NombreCompleto}' no tiene medicamento recurrente.", new List<object>());

            var resumen = new { ClienteId = cliente.IdCliente, NombreCliente = cliente.NombreCompleto, MedicamentoRecurrente = cliente.MedicamentoRecurrente, TieneTelegram = !string.IsNullOrWhiteSpace(cliente.ChatId) };
            return ResultadoOperacion.Exito($"Datos de alertas para '{cliente.NombreCompleto}'.", resumen);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }
}
