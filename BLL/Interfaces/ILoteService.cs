using BLL.DTOs;
using Entity;
using System;

namespace BLL.Interfaces;

/// <summary>
/// Contrato para la lógica de negocio relacionada con lotes e inventario.
/// </summary>
public interface ILoteService
{
    ResultadoOperacion ObtenerPorId(int id);
    ResultadoOperacion ObtenerPorProducto(int productoId);
    ResultadoOperacion ObtenerTodosActivos();
    ResultadoOperacion SeleccionarFEFO(int productoId);
    ResultadoOperacion InsertarLote(Lote lote);
    ResultadoOperacion ActualizarStock(int loteId, int cantidad, EstadoLote estado);
}