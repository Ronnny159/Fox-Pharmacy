using System;
using BLL.DTOs;

namespace BLL.Interfaces;

/// <summary>
/// Contrato para la lógica de negocio relacionada con usuarios y autenticación.
/// </summary>
public interface IUsuarioService
{
    ResultadoOperacion Autenticar(string nombreUsuario, string contrasena);
    ResultadoOperacion ObtenerPorId(int id);
    ResultadoOperacion ObtenerTodos();
}