using BLL.DTOs;
using Entity;

namespace BLL.Interfaces;

public interface ILoginService
{
    ResultadoOperacion Autenticar(string nombreUsuario, string contrasena);
    ResultadoOperacion Registrar(RegistroUsuarioDTO registroDTO);
    ResultadoOperacion ValidarCredenciales(string nombreUsuario, string contrasena);
}