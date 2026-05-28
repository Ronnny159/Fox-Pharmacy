using System;

namespace Entity;

/// <summary>
/// Parámetros de configuración global del sistema.
/// Permiten ajustar el comportamiento sin modificar código fuente.
/// Principio aplicado: Open/Closed (el sistema se adapta sin recompilar).
/// </summary>

public class ParametroSistema : BaseEntity
{
    public int IdParametro { get; set; }
    public string Clave { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
}
