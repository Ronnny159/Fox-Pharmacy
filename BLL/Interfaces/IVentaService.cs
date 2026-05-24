using System;
using BLL.DTOs;
using Entity;

namespace BLL.Interfaces;

/// <summary>
/// Contrato para la lógica de negocio relacionada con ventas y facturación.
/// </summary>
public interface IVentaService
{
    ResultadoOperacion RealizarVenta(List<(int productoId, int cantidad)> items, int usuarioId, int? clienteId = null);
    ResultadoOperacion ObtenerVentaPorId(int id);
    ResultadoOperacion ObtenerVentasPorUsuario(int usuarioId);
    ResultadoOperacion ObtenerVentasPorFechas(DateTime desde, DateTime hasta);
    ResultadoOperacion AnularVenta(int ventaId, int usuarioId);
}