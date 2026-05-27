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
        EjecutarProcedimiento("SP_INSERTAR_HISTORIAL_PARAMETRO", cmd =>
        {
            cmd.Parameters.Add("p_clave_parametro", OracleDbType.Varchar2).Value = historial.ClaveParametro;
            cmd.Parameters.Add("p_valor_anterior", OracleDbType.Varchar2).Value = historial.ValorAnterior;
            cmd.Parameters.Add("p_valor_nuevo", OracleDbType.Varchar2).Value = historial.ValorNuevo;
            cmd.Parameters.Add("p_motivo", OracleDbType.Varchar2).Value = historial.Motivo;
            cmd.Parameters.Add("p_modificado_por_id", OracleDbType.Int32).Value = historial.ModificadoPorId;
            cmd.Parameters.Add("p_direccion_ip", OracleDbType.Varchar2).Value = historial.DireccionIP ?? (object)DBNull.Value;
        });
    }

    public List<HistorialParametro> ObtenerPorParametro(string clave)
    {
        var lista = new List<HistorialParametro>();
        EjecutarCursor("SP_OBTENER_HISTORIAL_POR_PARAMETRO",
            cmd => cmd.Parameters.Add("p_clave", OracleDbType.Varchar2).Value = clave,
            reader => { while (reader.Read()) lista.Add(MapearHistorial(reader)); });
        return lista;
    }

    public List<HistorialParametro> ObtenerPorUsuario(int usuarioId)
    {
        var lista = new List<HistorialParametro>();
        EjecutarCursor("SP_OBTENER_HISTORIAL_POR_USUARIO",
            cmd => cmd.Parameters.Add("p_usuario_id", OracleDbType.Int32).Value = usuarioId,
            reader => { while (reader.Read()) lista.Add(MapearHistorial(reader)); });
        return lista;
    }

    public List<HistorialParametro> ObtenerPorRangoFechas(DateTime desde, DateTime hasta)
    {
        var lista = new List<HistorialParametro>();
        EjecutarCursor("SP_OBTENER_HISTORIAL_POR_FECHAS",
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
            Id = reader.GetInt32(reader.GetOrdinal("ID")),
            ClaveParametro = LeerString(reader, "CLAVE_PARAMETRO"),
            ValorAnterior = LeerString(reader, "VALOR_ANTERIOR"),
            ValorNuevo = LeerString(reader, "VALOR_NUEVO"),
            Motivo = LeerString(reader, "MOTIVO"),
            FechaCambio = reader.GetDateTime(reader.GetOrdinal("FECHA_CAMBIO")),
            ModificadoPorId = reader.GetInt32(reader.GetOrdinal("MODIFICADO_POR_ID")),
            DireccionIP = LeerString(reader, "DIRECCION_IP"),
            Activo = LeerBooleano(reader, "ACTIVO"),
            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
        };
    }
}