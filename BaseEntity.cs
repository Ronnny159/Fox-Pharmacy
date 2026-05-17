using System;

namespace Entities;

/// <summary>
/// Clase base abstracta para todas las entidades del sistema.
/// Proporciona identidad común y campos de auditoría.
/// Principio aplicado: DRY, Single Responsibility.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Identificador único de la entidad (clave primaria en base de datos).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Fecha y hora de creación del registro en el sistema.
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    /// <summary>
    /// Indicador de registro activo (true) o eliminado lógicamente (false).
    /// </summary>
    public bool Activo { get; set; } = true;
}