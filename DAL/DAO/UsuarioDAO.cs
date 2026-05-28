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
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_USUARIO_POR_CREDENCIALES",
            cmd =>
            {
                cmd.Parameters.Add("p_nombre", OracleDbType.Varchar2).Value = nombreUsuario;
                cmd.Parameters.Add("p_hash", OracleDbType.Varchar2).Value = hashContrasena;
            },
            reader => { if (reader.Read()) resultado = MapearUsuario(reader); });
        return resultado;
    }

    public Usuario? ObtenerPorId(int id)
    {
        Usuario? resultado = null;
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_USUARIO_POR_ID",
            cmd => cmd.Parameters.Add("p_id_usuario", OracleDbType.Int32).Value = id,
            reader => { if (reader.Read()) resultado = MapearUsuario(reader); });
        return resultado;
    }

    public Usuario? ObtenerPorDocumento(string documento)
    {
        Usuario? resultado = null;
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_USUARIO_POR_DOCUMENTO",
            cmd => cmd.Parameters.Add("p_doc", OracleDbType.Varchar2).Value = documento,
            reader => { if (reader.Read()) resultado = MapearUsuario(reader); });
        return resultado;
    }

    public List<Usuario> ObtenerTodos()
    {
        var lista = new List<Usuario>();
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_TODOS_USUARIOS",
            cmd => { },
            reader => { while (reader.Read()) lista.Add(MapearUsuario(reader)); });
        return lista;
    }

    public void Insertar(Usuario usuario)
    {
        EjecutarProcedimiento("PKG_PHARMASMART_CONFIG.INSERTAR_USUARIO", cmd =>
        {
            cmd.Parameters.Add("p_user", OracleDbType.Varchar2).Value = usuario.NombreUsuario;
            cmd.Parameters.Add("p_hash", OracleDbType.Varchar2).Value = usuario.HashContrasena;
            cmd.Parameters.Add("p_nombre", OracleDbType.Varchar2).Value = usuario.NombreCompleto;
            cmd.Parameters.Add("p_rol", OracleDbType.Char).Value = usuario.Rol;
            cmd.Parameters.Add("p_doc", OracleDbType.Varchar2).Value = usuario.DocumentoIdentidad;
        });
    }

    public void Actualizar(Usuario usuario)
    {
        EjecutarProcedimiento("PKG_PHARMASMART_CONFIG.ACTUALIZAR_USUARIO", cmd =>
        {
            cmd.Parameters.Add("p_id_usuario", OracleDbType.Int32).Value = usuario.IdUsuario;
            cmd.Parameters.Add("p_nombre", OracleDbType.Varchar2).Value = usuario.NombreCompleto;
            cmd.Parameters.Add("p_rol", OracleDbType.Char).Value = usuario.Rol;
            cmd.Parameters.Add("p_estado", OracleDbType.Char).Value = usuario.Estado;
        });
    }

    private Usuario MapearUsuario(OracleDataReader reader)
    {
        return new Usuario
        {
            IdUsuario = reader.GetInt32(reader.GetOrdinal("ID_USUARIO")),
            NombreUsuario = LeerString(reader, "NOMBRE_USUARIO"),
            HashContrasena = LeerString(reader, "HASH_CONTRASENA"),
            NombreCompleto = LeerString(reader, "NOMBRE_COMPLETO"),
            Rol = LeerChar(reader, "ROL"),
            DocumentoIdentidad = LeerString(reader, "DOCUMENTO_IDENTIDAD"),
            Estado = LeerChar(reader, "ESTADO"),
            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
        };
    }
}
