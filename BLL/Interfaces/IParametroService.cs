using System;
using BLL.DTOs;

namespace BLL.Interfaces;

/// <summary>
/// Contrato para la lógica de negocio relacionada con parámetros del sistema.
/// </summary>
public interface IParametroService
{
    decimal ObtenerPorcentajeDescuentoVencimiento();
    decimal ObtenerUmbralAlertaInflacion();
    int ObtenerDiasVentanaCritica();
    string ObtenerParametro(string clave);
    ResultadoOperacion ActualizarDescuentoGeneral(decimal nuevoPorcentaje, int usuarioId, string motivo, string? ip = null);
    ResultadoOperacion ActualizarDescuentoProducto(int productoId, decimal? descuento, int usuarioId, string motivo, string? ip = null);
}