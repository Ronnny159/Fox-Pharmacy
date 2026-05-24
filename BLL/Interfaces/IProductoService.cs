using BLL.DTOs;
using Entity;
using System;

namespace BLL.Interfaces;

/// <summary>
/// Contrato para la lógica de negocio relacionada con productos.
/// </summary>
public interface IProductoService
{
    ResultadoOperacion ObtenerTodos();
    ResultadoOperacion ObtenerPorCodigo(string codigo);
    ResultadoOperacion ObtenerPorId(int id);
    ResultadoOperacion ObtenerPorNombre(string nombre);
    ResultadoOperacion Insertar(Producto producto);
    ResultadoOperacion Actualizar(Producto producto);
    ResultadoOperacion Eliminar(int id);
}