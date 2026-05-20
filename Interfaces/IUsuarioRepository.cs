using System;
using System.Collections.Generic;
using Entities;

namespace DAL.Interfaces;

/// <summary>
/// Contrato para operaciones de acceso a datos de la entidad Usuario.
/// </summary>
public interface IUsuarioRepository
{
    Usuario? ObtenerPorCredenciales(string nombreUsuario, string hashContrasena);
    Usuario? ObtenerPorId(int id);
    Usuario? ObtenerPorDocumento(string documento);
    IEnumerable<Usuario> ObtenerTodos();
    void Insertar(Usuario usuario);
    void Actualizar(Usuario usuario);
}