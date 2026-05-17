using System;

namespace Entities
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string ContrasenaHash { get; set; }
        public string Rol { get; set; }
        public bool Activo { get; set; }
        public DateTime? UltimoAcceso { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Propiedades calculadas
        public bool EsAdmin => Rol == "ADMIN";
        public bool EsGerente => Rol == "GERENTE" || EsAdmin;
        public bool EsVendedor => Rol == "VENDEDOR" || EsGerente;
    }
}