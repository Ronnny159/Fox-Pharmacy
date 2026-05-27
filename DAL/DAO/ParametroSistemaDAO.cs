using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;

namespace DAL.DAO;

public class ParametroSistemaDAO : BaseDAO, IParametroSistemaDAO
{
    public ParametroSistemaDAO(IOracleConnectionFactory conexionFactory) : base(conexionFactory) { }

    public ParametroSistema? ObtenerPorClave(string clave)
    {
        ParametroSistema? resultado = null;
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_PARAMETRO",
            cmd => cmd.Parameters.Add("p_clave", OracleDbType.Varchar2).Value = clave,
            reader => { if (reader.Read()) resultado = MapearParametro(reader); });
        return resultado;
    }

    public List<ParametroSistema> ObtenerTodos()
    {
        var lista = new List<ParametroSistema>();
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_TODOS_PARAMETROS",
            cmd => { },
            reader => { while (reader.Read()) lista.Add(MapearParametro(reader)); });
        return lista;
    }

    public void Insertar(ParametroSistema parametro)
    {
        EjecutarProcedimiento("PKG_PHARMASMART_CONFIG.INSERTAR_PARAMETRO", cmd =>
        {
            cmd.Parameters.Add("p_clave", OracleDbType.Varchar2).Value = parametro.Clave;
            cmd.Parameters.Add("p_valor", OracleDbType.Varchar2).Value = parametro.Valor;
            cmd.Parameters.Add("p_descripcion", OracleDbType.Varchar2).Value = parametro.Descripcion ?? (object)DBNull.Value;
        });
    }

    public void Actualizar(ParametroSistema parametro)
    {
        EjecutarProcedimiento("PKG_PHARMASMART_CONFIG.ACTUALIZAR_PARAMETRO", cmd =>
        {
            cmd.Parameters.Add("p_clave", OracleDbType.Varchar2).Value = parametro.Clave;
            cmd.Parameters.Add("p_nuevo_valor", OracleDbType.Varchar2).Value = parametro.Valor;
            cmd.Parameters.Add("p_usuario_id", OracleDbType.Int32).Value = 1;
            cmd.Parameters.Add("p_motivo", OracleDbType.Varchar2).Value = "Actualización desde sistema";
            cmd.Parameters.Add("p_ip", OracleDbType.Varchar2).Value = "127.0.0.1";
        });
    }

    private ParametroSistema MapearParametro(OracleDataReader reader)
    {
        return new ParametroSistema
        {
            Id = reader.GetInt32(reader.GetOrdinal("ID")),
            Clave = LeerString(reader, "CLAVE"),
            Valor = LeerString(reader, "VALOR"),
            Descripcion = LeerString(reader, "DESCRIPCION"),
            Activo = LeerBooleano(reader, "ACTIVO"),
            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
        };
    }

    IEnumerable<ParametroSistema> IParametroSistemaDAO.ObtenerTodos()
    {
        return ObtenerTodos();
    }
}