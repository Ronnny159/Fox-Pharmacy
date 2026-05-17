using System;

namespace Entities;

/// <summary>
/// Parámetros de configuración global del sistema.
/// Permiten ajustar el comportamiento sin modificar código fuente.
/// Principio aplicado: Open/Closed (el sistema se adapta sin recompilar).
/// </summary>
public class ParametroSistema : BaseEntity
{
    /// <summary>
    /// Clave única que identifica el parámetro (ej: "PORCENTAJE_DESCUENTO_VENCIMIENTO").
    /// </summary>
    public string Clave { get; set; } = string.Empty;

    /// <summary>
    /// Valor del parámetro en formato string. Se convierte al tipo necesario al consumirlo.
    /// </summary>
    public string Valor { get; set; } = string.Empty;

    /// <summary>
    /// Descripción legible del propósito del parámetro.
    /// </summary>
    public string Descripcion { get; set; } = string.Empty;
}