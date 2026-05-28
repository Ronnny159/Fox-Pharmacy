using System;

namespace Entity;

/// <summary>
/// Representa a los usuarios del sistema: cajeros, administradores,
/// farmacéuticos y auditores.
/// </summary>

public class Usuario : BaseEntity
{
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string HashContrasena { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public char Rol { get; set; } = '2';
    public string DocumentoIdentidad { get; set; } = string.Empty;
    public char Estado { get; set; } = 'A';

    public bool EsAdministrador => Rol == '1';
    public bool PuedeModificarConfiguracion => Rol == '1';
    public bool EstaActivo => Estado == 'A';

    public virtual ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    public virtual ICollection<AjusteInventario> Ajustes { get; set; } = new List<AjusteInventario>();
}