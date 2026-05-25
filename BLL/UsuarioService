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
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    /// <summary>
    /// Autentica un usuario verificando sus credenciales contra la base de datos.
    /// La contraseña se hashea con SHA-256 en la BLL antes de enviarse al repositorio;
    /// nunca viaja en texto plano hacia la capa de datos.
    /// Retorna el usuario sin el hash de contraseña para que la UI no lo exponga.
    /// </summary>
    public ResultadoOperacion Autenticar(string nombreUsuario, string contrasena)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                return ResultadoOperacion.Fallo("El nombre de usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(contrasena))
                return ResultadoOperacion.Fallo("La contraseña es obligatoria.");

            string hashContrasena = CalcularHashSHA256(contrasena);

            var usuario = _usuarioRepository.ObtenerPorCredenciales(
                nombreUsuario.Trim().ToLower(),
                hashContrasena);

            if (usuario is null)
                return ResultadoOperacion.Fallo(
                    "Credenciales incorrectas. Verifique su usuario y contraseña.");

            if (!usuario.Activo)
                return ResultadoOperacion.Fallo(
                    "La cuenta de usuario está desactivada. Contacte al administrador.");

            // Proyectar sin exponer el hash de contraseña hacia la UI
            var sesion = new
            {
                usuario.Id,
                usuario.NombreUsuario,
                usuario.NombreCompleto,
                usuario.DocumentoIdentidad,
                Rol = usuario.Rol.ToString(),
                usuario.EsAdministrador,
                usuario.PuedeModificarConfiguracion
            };

            return ResultadoOperacion.Exito(
                $"Bienvenido, {usuario.NombreCompleto}.",
                sesion);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Obtiene los datos de un usuario por su identificador interno.
    /// No incluye el hash de contraseña en el resultado.
    /// </summary>
    public ResultadoOperacion ObtenerPorId(int id)
    {
        try
        {
            if (id <= 0)
                return ResultadoOperacion.Fallo("El identificador del usuario no es válido.");

            var usuario = _usuarioRepository.ObtenerPorId(id);

            if (usuario is null)
                return ResultadoOperacion.Fallo(
                    $"No se encontró ningún usuario con el ID {id}.");

            var resultado = new
            {
                usuario.Id,
                usuario.NombreUsuario,
                usuario.NombreCompleto,
                usuario.DocumentoIdentidad,
                Rol = usuario.Rol.ToString(),
                usuario.EsAdministrador,
                usuario.PuedeModificarConfiguracion,
                usuario.Activo
            };

            return ResultadoOperacion.Exito(
                $"Usuario '{usuario.NombreCompleto}' encontrado.",
                resultado);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Retorna todos los usuarios activos del sistema.
    /// No incluye hashes de contraseña en el resultado.
    /// </summary>
    public ResultadoOperacion ObtenerTodos()
    {
        try
        {
            var usuarios = _usuarioRepository.ObtenerTodos().ToList();

            if (usuarios.Count == 0)
                return ResultadoOperacion.Exito(
                    "No hay usuarios registrados en el sistema.",
                    new List<object>());

            var resultado = usuarios.Select(u => new
            {
                u.Id,
                u.NombreUsuario,
                u.NombreCompleto,
                u.DocumentoIdentidad,
                Rol = u.Rol.ToString(),
                u.EsAdministrador,
                u.Activo
            }).ToList();

            return ResultadoOperacion.Exito(
                $"Se encontraron {resultado.Count} usuario(s) en el sistema.",
                resultado);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Calcula el hash SHA-256 de una contraseña en texto plano.
    /// Retorna el hash en formato hexadecimal en minúsculas.
    /// </summary>
    private static string CalcularHashSHA256(string contrasena)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(contrasena));

        var sb = new StringBuilder(bytes.Length * 2);
        foreach (byte b in bytes)
            sb.Append(b.ToString("x2"));

        return sb.ToString();
    }
}
