using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;

namespace DAL.DAO;

public class AjusteInventarioDAO : BaseDAO, IAjusteInventarioDAO
{
    public AjusteInventarioDAO(IOracleConnectionFactory conexionFactory) : base(conexionFactory) { }

    public void Insertar(AjusteInventario ajuste)
    {
        EjecutarProcedimiento("PKG_PHARMASMART_CONFIG.REGISTRAR_AJUSTE", cmd =>
        {
            cmd.Parameters.Add("p_lote_id", OracleDbType.Int32).Value = ajuste.LoteId;
            cmd.Parameters.Add("p_tipo", OracleDbType.Varchar2).Value = ajuste.Tipo.ToString();
            cmd.Parameters.Add("p_cantidad", OracleDbType.Int32).Value = ajuste.Cantidad;
            cmd.Parameters.Add("p_motivo", OracleDbType.Varchar2).Value = ajuste.Motivo;
            cmd.Parameters.Add("p_responsable_id", OracleDbType.Int32).Value = ajuste.ResponsableId;
        });
    }

    public List<AjusteInventario> ObtenerPorLote(int loteId)
    {
        var lista = new List<AjusteInventario>();
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_AJUSTES_POR_LOTE",
            cmd => cmd.Parameters.Add("p_lote_id", OracleDbType.Int32).Value = loteId,
            reader => { while (reader.Read()) lista.Add(MapearAjuste(reader)); });
        return lista;
    }

    public List<AjusteInventario> ObtenerPorResponsable(int usuarioId)
    {
        var lista = new List<AjusteInventario>();
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_AJUSTES_POR_RESPONSABLE",
            cmd => cmd.Parameters.Add("p_usuario_id", OracleDbType.Int32).Value = usuarioId,
            reader => { while (reader.Read()) lista.Add(MapearAjuste(reader)); });
        return lista;
    }

    public List<AjusteInventario> ObtenerPorRangoFechas(DateTime desde, DateTime hasta)
    {
        var lista = new List<AjusteInventario>();
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_AJUSTES_POR_FECHAS",
            cmd =>
            {
                cmd.Parameters.Add("p_desde", OracleDbType.Date).Value = desde;
                cmd.Parameters.Add("p_hasta", OracleDbType.Date).Value = hasta;
            },
            reader => { while (reader.Read()) lista.Add(MapearAjuste(reader)); });
        return lista;
    }

    private AjusteInventario MapearAjuste(OracleDataReader reader)
    {
        return new AjusteInventario
        {
            Id = reader.GetInt32(reader.GetOrdinal("ID")),
            LoteId = reader.GetInt32(reader.GetOrdinal("LOTE_ID")),
            Tipo = Enum.Parse<TipoAjuste>(LeerString(reader, "TIPO")),
            Cantidad = reader.GetInt32(reader.GetOrdinal("CANTIDAD")),
            Motivo = LeerString(reader, "MOTIVO"),
            FechaAjuste = reader.GetDateTime(reader.GetOrdinal("FECHA_AJUSTE")),
            ResponsableId = reader.GetInt32(reader.GetOrdinal("RESPONSABLE_ID")),
            Activo = LeerBooleano(reader, "ACTIVO"),
            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
        };
    }
}