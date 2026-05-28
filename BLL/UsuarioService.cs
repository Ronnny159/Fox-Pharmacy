using BLL.DTOs;
using BLL.Interfaces;
using DAL.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Services;

/// <summary>
/// Lógica de negocio para autenticación y gestión de usuarios del sistema.
/// Nunca expone contraseñas ni hashes hacia la UI.
/// El hash de contraseñas se calcula en esta capa antes de consultar la BD.
/// </summary>

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioDAO _usuarioDAO;

    public UsuarioService(IUsuarioDAO usuarioDAO)
    {
        _usuarioDAO = usuarioDAO;
    }

    public ResultadoOperacion Autenticar(string nombreUsuario, string contrasena)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                return ResultadoOperacion.Fallo("El nombre de usuario es obligatorio.");
            if (string.IsNullOrWhiteSpace(contrasena))
                return ResultadoOperacion.Fallo("La contraseña es obligatoria.");

            string hashContrasena = CalcularHashSHA256(contrasena);
            var usuario = _usuarioDAO.ObtenerPorCredenciales(nombreUsuario.Trim().ToLower(), hashContrasena);

            if (usuario is null)
                return ResultadoOperacion.Fallo("Credenciales incorrectas.");
            if (usuario.Estado != 'A')
                return ResultadoOperacion.Fallo("La cuenta está desactivada.");

            var sesion = new
            {
                usuario.IdUsuario,
                usuario.NombreUsuario,
                usuario.NombreCompleto,
                usuario.DocumentoIdentidad,
                Rol = usuario.Rol,
                usuario.EsAdministrador,
                usuario.PuedeModificarConfiguracion
            };

            return ResultadoOperacion.Exito($"Bienvenido, {usuario.NombreCompleto}.", sesion);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    public ResultadoOperacion ObtenerPorId(int id)
    {
        try
        {
            if (id <= 0) return ResultadoOperacion.Fallo("ID no válido.");
            var usuario = _usuarioDAO.ObtenerPorId(id);
            if (usuario is null) return ResultadoOperacion.Fallo($"Usuario con ID {id} no encontrado.");
            return ResultadoOperacion.Exito($"Usuario '{usuario.NombreCompleto}' encontrado.", usuario);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ObtenerTodos()
    {
        try
        {
            var usuarios = _usuarioDAO.ObtenerTodos();
            if (usuarios.Count == 0) return ResultadoOperacion.Exito("No hay usuarios.", new List<object>());
            return ResultadoOperacion.Exito($"{usuarios.Count} usuario(s) encontrados.", usuarios);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    private static string CalcularHashSHA256(string contrasena)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(contrasena));
        var sb = new StringBuilder(bytes.Length * 2);
        foreach (byte b in bytes) sb.Append(b.ToString("x2"));
        return sb.ToString();
    }
}
