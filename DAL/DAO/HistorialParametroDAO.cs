using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;

namespace DAL.DAO;

public class HistorialParametroDAO : BaseDAO, IHistorialParametroDAO
{
    public HistorialParametroDAO(IOracleConnectionFactory conexionFactory) : base(conexionFactory) { }

    public void Insertar(HistorialParametro historial)
    {
        EjecutarProcedimiento("PKG_PHARMASMART_ALERTAS.INSERTAR_HISTORIAL_PARAMETRO", cmd =>
        {
            cmd.Parameters.Add("p_clave_parametro", OracleDbType.Varchar2).Value = historial.ClaveParametro;
            cmd.Parameters.Add("p_valor_anterior", OracleDbType.Varchar2).Value = historial.ValorAnterior;
            cmd.Parameters.Add("p_valor_nuevo", OracleDbType.Varchar2).Value = historial.ValorNuevo;
            cmd.Parameters.Add("p_motivo", OracleDbType.Varchar2).Value = historial.Motivo;
            cmd.Parameters.Add("p_id_usuario", OracleDbType.Int32).Value = historial.IdUsuario;
            cmd.Parameters.Add("p_ip", OracleDbType.Varchar2).Value = historial.DireccionIP ?? (object)DBNull.Value;
        });
    }

    public List<HistorialParametro> ObtenerPorParametro(string clave)
    {
        var lista = new List<HistorialParametro>();
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_HISTORIAL_PARAMETROS",
            cmd => cmd.Parameters.Add("p_clave", OracleDbType.Varchar2).Value = clave,
            reader => { while (reader.Read()) lista.Add(MapearHistorial(reader)); });
        return lista;
    }

    public List<HistorialParametro> ObtenerPorUsuario(int usuarioId)
    {
        var lista = new List<HistorialParametro>();
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_HISTORIAL_POR_USUARIO",
            cmd => cmd.Parameters.Add("p_id_usuario", OracleDbType.Int32).Value = usuarioId,
            reader => { while (reader.Read()) lista.Add(MapearHistorial(reader)); });
        return lista;
    }

    public List<HistorialParametro> ObtenerPorRangoFechas(DateTime desde, DateTime hasta)
    {
        var lista = new List<HistorialParametro>();
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_HISTORIAL_POR_FECHAS",
            cmd =>
            {
                cmd.Parameters.Add("p_desde", OracleDbType.Date).Value = desde;
                cmd.Parameters.Add("p_hasta", OracleDbType.Date).Value = hasta;
            },
            reader => { while (reader.Read()) lista.Add(MapearHistorial(reader)); });
        return lista;
    }

    private HistorialParametro MapearHistorial(OracleDataReader reader)
    {
        return new HistorialParametro
        {
            IdHistorial = reader.GetInt32(reader.GetOrdinal("ID_HISTORIAL")),
            ClaveParametro = LeerString(reader, "CLAVE_PARAMETRO"),
            ValorAnterior = LeerString(reader, "VALOR_ANTERIOR"),
            ValorNuevo = LeerString(reader, "VALOR_NUEVO"),
            Motivo = LeerString(reader, "MOTIVO"),
            FechaCambio = reader.GetDateTime(reader.GetOrdinal("FECHA_CAMBIO")),
            IdUsuario = reader.GetInt32(reader.GetOrdinal("ID_USUARIO")),
            DireccionIP = LeerString(reader, "DIRECCION_IP")
        };
    }
}