using Entity;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces;

/// <summary>
/// Contrato para operaciones de acceso a datos de la entidad Usuario.
/// </summary>
public interface IUsuarioRepository
{
    Usuario? ObtenerPorCredencialesAsync(string nombreUsuario, string hashContrasena);
    Usuario? ObtenerPorId(int id);
    Usuario? ObtenerPorDocumento(string documento);
    IEnumerable<Usuario> ObtenerTodos();
    void Insertar(Usuario usuario);
    void Actualizar(Usuario usuario);
}