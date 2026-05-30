using BLL.DTOs;
using BLL.Interfaces;
using DAL.Interfaces;
using Entity;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Services;

public class LoginService : ILoginService
{
    private readonly IUsuarioDAO _usuarioDAO;

    public LoginService(IUsuarioDAO usuarioDAO)
    {
        _usuarioDAO = usuarioDAO;
    }
    public ResultadoOperacion Autenticar(string nombreUsuario, string contrasena)
    {
        try
        {
            if(string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(contrasena))
                return ResultadoOperacion.Fallo("El nombre de usuario y la contraseña no pueden estar vacíos.");
            if(string.IsNullOrWhiteSpace(contrasena) || contrasena.Length < 6)
                return ResultadoOperacion.Fallo("La contraseña debe tener al menos 6 caracteres.");
            
            string hashContrasena = CalcularHashSHA256(contrasena);
            var usuario = _usuarioDAO.ObtenerPorCredenciales(nombreUsuario.Trim().ToLower(), hashContrasena);
            if (usuario == null)
                return ResultadoOperacion.Fallo("Nombre de usuario o contraseña incorrectos.");
        
            var sesion = new
            {
                usuario.IdUsuario,
                usuario.NombreUsuario,
                usuario.NombreCompleto,
                usuario.DocumentoIdentidad,
                Rol = usuario.Rol,
                EsAdministrador = usuario.Rol == '1',
                PuedeModificarConfiguracion = usuario.Rol == '1'
            };

            return ResultadoOperacion.Exito("Usuario autenticado correctamente.", sesion);
        
        }
        catch (Exception)
        {
            return ResultadoOperacion.Fallo("Error al autenticar el usuario.");
        }
    }

    public ResultadoOperacion Registrar(RegistroUsuarioDTO registroDTO)
    {
        try
        {
            // Validaciones
            if (registroDTO is null)
                return ResultadoOperacion.Fallo("Datos de registro obligatorios.");

            if (string.IsNullOrWhiteSpace(registroDTO.PrimerNombre))
                return ResultadoOperacion.Fallo("El primer nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(registroDTO.PrimerApellido))
                return ResultadoOperacion.Fallo("El primer apellido es obligatorio.");

            if (string.IsNullOrWhiteSpace(registroDTO.NombreUsuario))
                return ResultadoOperacion.Fallo("El nombre de usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(registroDTO.Contrasena))
                return ResultadoOperacion.Fallo("La contraseña es obligatoria.");

            if (registroDTO.Contrasena.Length < 6)
                return ResultadoOperacion.Fallo("La contraseña debe tener al menos 6 caracteres.");

            if (string.IsNullOrWhiteSpace(registroDTO.DocumentoIdentidad))
                return ResultadoOperacion.Fallo("El documento de identidad es obligatorio.");

            // Verificar si el nombre de usuario ya existe
            var usuarioExistente = _usuarioDAO.ObtenerPorCredenciales(
                registroDTO.NombreUsuario.Trim().ToLower(), 
                "no_match_will_return_null");
            
            if (usuarioExistente is not null)
                return ResultadoOperacion.Fallo($"El nombre de usuario '{registroDTO.NombreUsuario}' ya está en uso.");

            // Verificar si el documento ya está registrado
            var usuarioPorDocumento = _usuarioDAO.ObtenerPorDocumento(registroDTO.DocumentoIdentidad.Trim());
            if (usuarioPorDocumento is not null)
                return ResultadoOperacion.Fallo($"Ya existe un usuario con documento '{registroDTO.DocumentoIdentidad}'.");

            // Crear el nuevo usuario
            var nuevoUsuario = new Usuario
            {
                NombreUsuario = registroDTO.NombreUsuario.Trim().ToLower(),
                HashContrasena = CalcularHashSHA256(registroDTO.Contrasena),
                NombreCompleto = registroDTO.NombreCompleto,
                DocumentoIdentidad = registroDTO.DocumentoIdentidad.Trim(),
                Rol = registroDTO.Rol,
                Estado = 'A',
                FechaCreacion = DateTime.Now
            };

            _usuarioDAO.Insertar(nuevoUsuario);

            return ResultadoOperacion.Exito(
                $"¡Registro exitoso! Bienvenido {nuevoUsuario.NombreCompleto}. Ya puedes iniciar sesión.", 
                new { nuevoUsuario.IdUsuario, nuevoUsuario.NombreUsuario });
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo($"Error al registrar: {ex.Message}");
        }
    }

    public ResultadoOperacion ValidarCredenciales(string nombreUsuario, string contrasena)
    {
       return Autenticar(nombreUsuario, contrasena);
    }

    private static string CalcularHashSHA256(string contrasena)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(contrasena));
        var sb = new StringBuilder(bytes.Length * 2);
        foreach (byte b in bytes)
            sb.Append(b.ToString("x2"));
        return sb.ToString();
    }
    
}
