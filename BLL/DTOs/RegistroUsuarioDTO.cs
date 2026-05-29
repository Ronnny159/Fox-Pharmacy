using System;

namespace BLL.DTOs;

public class RegistroUsuarioDTO
{
    public string PrimerNombre { get; set; } = string.Empty;
    public string SegundoNombre { get; set; }
    public string PrimerApellido { get; set; } = string.Empty;
    public string SegundoApellido { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
    public string DocumentoIdentidad { get; set; } = string.Empty;
    public char Rol { get; set; } = '2';
    public string NombreCompleto { get; internal set; }
}