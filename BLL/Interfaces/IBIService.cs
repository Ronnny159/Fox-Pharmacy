using System;
using BLL.DTOs;

namespace BLL.Interfaces;

/// <summary>
/// Contrato para la lógica de negocio relacionada con Business Intelligence.
/// </summary>
public interface IBIService
{
    ResultadoOperacion ObtenerTopProductos(int top = 10, string orden = "MAYOR");
    ResultadoOperacion ObtenerResumenVentas(DateTime desde, DateTime hasta);
    ResultadoOperacion ObtenerProductosPorVencer(int dias = 30);
    ResultadoOperacion ObtenerResumenInventario();
}