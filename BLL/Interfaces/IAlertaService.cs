using System;
using BLL.DTOs;

namespace BLL.Interfaces;

/// <summary>
/// Contrato para la lógica de negocio relacionada con alertas del sistema.
/// </summary>
public interface IAlertaService
{
    ResultadoOperacion ObtenerAlertasInflacionPendientes();
    ResultadoOperacion MarcarAlertaAtendida(int alertaId, int usuarioId);
    ResultadoOperacion ActualizarLotesVencidos();
    ResultadoOperacion EnviarAlertaFidelizacion(int clienteId, string tipo, string mensaje, int? productoId = null, int? loteId = null);
    ResultadoOperacion ObtenerAlertasCliente(int clienteId);
}