using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;

namespace DAL.DAO;

public class UsuarioDAO : BaseDAO, IUsuarioDAO
{
    public UsuarioDAO(IOracleConnectionFactory conexionFactory) : base(conexionFactory) { }

    public Usuario? ObtenerPorCredenciales(string nombreUsuario, string hashContrasena)
    {
        Usuario? resultado = null;
        EjecutarCursor("SP_OBTENER_USUARIO_POR_CREDENCIALES",
            cmd =>
            {
                cmd.Parameters.Add("p_nombre_usuario", OracleDbType.Varchar2).Value = nombreUsuario;
                cmd.Parameters.Add("p_hash_contrasena", OracleDbType.Varchar2).Value = hashContrasena;
            },
            reader => { if (reader.Read()) resultado = MapearUsuario(reader); });
        return resultado;
    }

    public Usuario? ObtenerPorId(int id)
    {
        Usuario? resultado = null;
        EjecutarCursor("SP_OBTENER_USUARIO_POR_ID",
            cmd => cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = id,
            reader => { if (reader.Read()) resultado = MapearUsuario(reader); });
        return resultado;
    }

    public Usuario? ObtenerPorDocumento(string documento)
    {
        Usuario? resultado = null;
        EjecutarCursor("SP_OBTENER_USUARIO_POR_DOCUMENTO",
            cmd => cmd.Parameters.Add("p_documento", OracleDbType.Varchar2).Value = documento,
            reader => { if (reader.Read()) resultado = MapearUsuario(reader); });
        return resultado;
    }

    public List<Usuario> ObtenerTodos()
    {
        var lista = new List<Usuario>();
        EjecutarCursor("SP_OBTENER_TODOS_USUARIOS",
            cmd => { },
            reader => { while (reader.Read()) lista.Add(MapearUsuario(reader)); });
        return lista;
    }

    public void Insertar(Usuario usuario)
    {
        EjecutarProcedimiento("SP_INSERTAR_USUARIO", cmd =>
        {
            cmd.Parameters.Add("p_nombre_usuario", OracleDbType.Varchar2).Value = usuario.NombreUsuario;
            cmd.Parameters.Add("p_hash_contrasena", OracleDbType.Varchar2).Value = usuario.HashContrasena;
            cmd.Parameters.Add("p_nombre_completo", OracleDbType.Varchar2).Value = usuario.NombreCompleto;
            cmd.Parameters.Add("p_rol", OracleDbType.Int32).Value = (int)usuario.Rol;
            cmd.Parameters.Add("p_documento", OracleDbType.Varchar2).Value = usuario.DocumentoIdentidad;
        });
    }

    public void Actualizar(Usuario usuario)
    {
        EjecutarProcedimiento("SP_ACTUALIZAR_USUARIO", cmd =>
        {
            cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = usuario.Id;
            cmd.Parameters.Add("p_nombre_completo", OracleDbType.Varchar2).Value = usuario.NombreCompleto;
            cmd.Parameters.Add("p_rol", OracleDbType.Int32).Value = (int)usuario.Rol;
            cmd.Parameters.Add("p_activo", OracleDbType.Int32).Value = usuario.Activo ? 1 : 0;
        });
    }

    private Usuario MapearUsuario(OracleDataReader reader)
    {
        return new Usuario
        {
            Id = reader.GetInt32(reader.GetOrdinal("ID")),
            NombreUsuario = LeerString(reader, "NOMBRE_USUARIO"),
            HashContrasena = LeerString(reader, "HASH_CONTRASENA"),
            NombreCompleto = LeerString(reader, "NOMBRE_COMPLETO"),
            Rol = (RolUsuario)reader.GetInt32(reader.GetOrdinal("ROL")),
            DocumentoIdentidad = LeerString(reader, "DOCUMENTO_IDENTIDAD"),
            Activo = LeerBooleano(reader, "ACTIVO"),
            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
        };
    }

    public Task<Usuario?> ObtenerPorCredencialesAsync(string nombreUsuario, string hashContrasena)
    {
        throw new NotImplementedException();
    }

    IEnumerable<Usuario> IUsuarioDAO.ObtenerTodos()
    {
        return ObtenerTodos();
    }
}
