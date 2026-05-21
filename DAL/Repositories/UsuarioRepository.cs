using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;

namespace DAL.Repositories;

public class UsuarioRepository : BaseRepository, IUsuarioRepository
{
    public UsuarioRepository(IOracleConnectionFactory conexionFactory)
        : base(conexionFactory) { }

    public Usuario? ObtenerPorCredenciales(string nombreUsuario, string hashContrasena)
    {
        const string sql = @"
            SELECT ID, NOMBRE_USUARIO, HASH_CONTRASENA, NOMBRE_COMPLETO, ROL, DOCUMENTO_IDENTIDAD,
                   ACTIVO, FECHA_CREACION
            FROM USUARIO 
            WHERE NOMBRE_USUARIO = :user AND HASH_CONTRASENA = :hash AND ACTIVO = 1";

        return EjecutarConsulta(sql, cmd =>
        {
            cmd.Parameters.Add("user", OracleDbType.Varchar2).Value = nombreUsuario;
            cmd.Parameters.Add("hash", OracleDbType.Varchar2).Value = hashContrasena;
        }, MapearUsuario);
    }

    public Usuario? ObtenerPorId(int id)
    {
        const string sql = @"
            SELECT ID, NOMBRE_USUARIO, HASH_CONTRASENA, NOMBRE_COMPLETO, ROL, DOCUMENTO_IDENTIDAD,
                   ACTIVO, FECHA_CREACION
            FROM USUARIO 
            WHERE ID = :id";

        return EjecutarConsulta(sql,
            cmd => cmd.Parameters.Add("id", OracleDbType.Int32).Value = id,
            MapearUsuario);
    }

    public Usuario? ObtenerPorDocumento(string documento)
    {
        const string sql = @"
            SELECT ID, NOMBRE_USUARIO, HASH_CONTRASENA, NOMBRE_COMPLETO, ROL, DOCUMENTO_IDENTIDAD,
                   ACTIVO, FECHA_CREACION
            FROM USUARIO 
            WHERE DOCUMENTO_IDENTIDAD = :doc";

        return EjecutarConsulta(sql,
            cmd => cmd.Parameters.Add("doc", OracleDbType.Varchar2).Value = documento,
            MapearUsuario);
    }

    public IEnumerable<Usuario> ObtenerTodos()
    {
        const string sql = @"
            SELECT ID, NOMBRE_USUARIO, HASH_CONTRASENA, NOMBRE_COMPLETO, ROL, DOCUMENTO_IDENTIDAD,
                   ACTIVO, FECHA_CREACION
            FROM USUARIO 
            WHERE ACTIVO = 1
            ORDER BY NOMBRE_COMPLETO";

        return EjecutarConsultaLista(sql, cmd => { }, MapearUsuario);
    }

    public void Insertar(Usuario usuario)
    {
        const string sql = @"
            INSERT INTO USUARIO (NOMBRE_USUARIO, HASH_CONTRASENA, NOMBRE_COMPLETO, ROL, DOCUMENTO_IDENTIDAD)
            VALUES (:user, :hash, :nombre, :rol, :documento)";

        EjecutarComando(sql, cmd =>
        {
            cmd.Parameters.Add("user", OracleDbType.Varchar2).Value = usuario.NombreUsuario;
            cmd.Parameters.Add("hash", OracleDbType.Varchar2).Value = usuario.HashContrasena;
            cmd.Parameters.Add("nombre", OracleDbType.Varchar2).Value = usuario.NombreCompleto;
            cmd.Parameters.Add("rol", OracleDbType.Int32).Value = (int)usuario.Rol;
            cmd.Parameters.Add("documento", OracleDbType.Varchar2).Value = usuario.DocumentoIdentidad;
        });
    }

    public void Actualizar(Usuario usuario)
    {
        const string sql = @"
            UPDATE USUARIO 
            SET NOMBRE_COMPLETO = :nombre, ROL = :rol, ACTIVO = :activo
            WHERE ID = :id";

        EjecutarComando(sql, cmd =>
        {
            cmd.Parameters.Add("id", OracleDbType.Int32).Value = usuario.Id;
            cmd.Parameters.Add("nombre", OracleDbType.Varchar2).Value = usuario.NombreCompleto;
            cmd.Parameters.Add("rol", OracleDbType.Int32).Value = (int)usuario.Rol;
            cmd.Parameters.Add("activo", OracleDbType.Int16).Value = usuario.Activo ? 1 : 0;
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
}