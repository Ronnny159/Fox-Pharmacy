using System;

namespace BLL.DTOs;

/// <summary>
/// DTO estándar para retorno de operaciones en la capa BLL.
/// Evita excepciones no controladas hacia la UI.
/// Principio aplicado: Separación de responsabilidades.
/// </summary>

public class ResultadoOperacion
{
    public bool Exitoso { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public object? Datos { get; set; }

    public static ResultadoOperacion Exito(string mensaje = "", object? datos = null)
        => new() { Exitoso = true, Mensaje = mensaje, Datos = datos };

    public static ResultadoOperacion Fallo(string mensaje)
        => new() { Exitoso = false, Mensaje = mensaje };

    public static ResultadoOperacion Fallo(Exception ex)
        => new() { Exitoso = false, Mensaje = $"Error: {ex.Message}" };
}