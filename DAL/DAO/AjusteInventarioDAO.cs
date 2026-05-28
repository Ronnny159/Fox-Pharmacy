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
        char tipoChar = ajuste.Tipo switch
        {
            TipoAjuste.Averia => 'A',
            TipoAjuste.Vencido => 'V',
            TipoAjuste.RetiroLegal => 'R',
            TipoAjuste.ConteoCiclico => 'C',
            _ => 'A'
        };

        EjecutarProcedimiento("PKG_PHARMASMART_CONFIG.REGISTRAR_AJUSTE", cmd =>
        {
            cmd.Parameters.Add("p_id_lote", OracleDbType.Int32).Value = ajuste.IdLote;
            cmd.Parameters.Add("p_tipo", OracleDbType.Char).Value = tipoChar;
            cmd.Parameters.Add("p_cantidad", OracleDbType.Int32).Value = ajuste.Cantidad;
            cmd.Parameters.Add("p_motivo", OracleDbType.Varchar2).Value = ajuste.Motivo;
            cmd.Parameters.Add("p_id_responsable", OracleDbType.Int32).Value = ajuste.IdResponsable;
        });
    }

    public List<AjusteInventario> ObtenerPorLote(int loteId)
    {
        var lista = new List<AjusteInventario>();
        EjecutarCursor("PKG_PHARMASMART_VENTAS.OBTENER_AJUSTES_POR_LOTE",
            cmd => cmd.Parameters.Add("p_id_lote", OracleDbType.Int32).Value = loteId,
            reader => { while (reader.Read()) lista.Add(MapearAjuste(reader)); });
        return lista;
    }

    public List<AjusteInventario> ObtenerPorResponsable(int usuarioId)
    {
        var lista = new List<AjusteInventario>();
        EjecutarCursor("PKG_PHARMASMART_VENTAS.OBTENER_AJUSTES_POR_RESPONSABLE",
            cmd => cmd.Parameters.Add("p_id_usuario", OracleDbType.Int32).Value = usuarioId,
            reader => { while (reader.Read()) lista.Add(MapearAjuste(reader)); });
        return lista;
    }

    public List<AjusteInventario> ObtenerPorRangoFechas(DateTime desde, DateTime hasta)
    {
        var lista = new List<AjusteInventario>();
        EjecutarCursor("PKG_PHARMASMART_VENTAS.OBTENER_AJUSTES_POR_FECHAS",
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
            IdAjuste = reader.GetInt32(reader.GetOrdinal("ID_AJUSTE")),
            IdLote = reader.GetInt32(reader.GetOrdinal("ID_LOTE")),
            Tipo = LeerChar(reader, "TIPO") switch
            {
                'A' => TipoAjuste.Averia,
                'V' => TipoAjuste.Vencido,
                'R' => TipoAjuste.RetiroLegal,
                'C' => TipoAjuste.ConteoCiclico,
                _ => TipoAjuste.Averia
            },
            Cantidad = reader.GetInt32(reader.GetOrdinal("CANTIDAD")),
            Motivo = LeerString(reader, "MOTIVO"),
            FechaAjuste = reader.GetDateTime(reader.GetOrdinal("FECHA_AJUSTE")),
            IdResponsable = reader.GetInt32(reader.GetOrdinal("ID_RESPONSABLE"))
        };
    }
}