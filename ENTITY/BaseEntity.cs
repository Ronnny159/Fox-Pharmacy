using System;

namespace Entity;

/// <summary>
/// Clase base abstracta para todas las entidades del sistema.
/// </summary>
public abstract class BaseEntity
{
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public bool Activo { get; set; } = true;
}
