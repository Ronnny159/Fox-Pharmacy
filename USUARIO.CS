using System;

namespace Entity;

/// <summary>
/// Representa a los usuarios del sistema: cajeros, administradores,
/// farmacéuticos y auditores.
/// </summary>
public class Usuario : BaseEntity
{
    /// <summary>
    /// Nombre de usuario para autenticación. Único en el sistema.
    /// </summary>
    public string NombreUsuario { get; set; } = string.Empty;

    /// <summary>
    /// Hash de la contraseña. Nunca se almacena en texto plano.
    /// </summary>
    public string HashContrasena { get; set; } = string.Empty;

    /// <summary>
    /// Nombre completo del usuario para registros, facturación y auditorías.
    /// </summary>
    public string NombreCompleto { get; set; } = string.Empty;

    /// <summary>
    /// Rol que determina los permisos del usuario en el sistema.
    /// </summary>
    public RolUsuario Rol { get; set; } = RolUsuario.Cajero;

    /// <summary>
    /// Número de documento de identidad para trazabilidad de acciones.
    /// </summary>
    public string DocumentoIdentidad { get; set; } = string.Empty;

    // Propiedades navegacionales
    public virtual ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    public virtual ICollection<AjusteInventario> Ajustes { get; set; } = new List<AjusteInventario>();

    /// <summary>
    /// Indica si el usuario tiene rol de administrador.
    /// </summary>
    public bool EsAdministrador => Rol == RolUsuario.Administrador;

    /// <summary>
    /// Indica si el usuario puede modificar configuraciones del sistema.
    /// </summary>
    public bool PuedeModificarConfiguracion => Rol == RolUsuario.Administrador;
}