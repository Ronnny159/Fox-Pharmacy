using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;

namespace DAL.Repositories;

public class AjusteInventarioRepository : BaseRepository, IAjusteInventarioRepository
{
    public AjusteInventarioRepository(IOracleConnectionFactory conexionFactory)
        : base(conexionFactory) { }

    public void Insertar(AjusteInventario ajuste)
    {
        const string sql = @"
            INSERT INTO AJUSTE_INVENTARIO (LOTE_ID, TIPO, CANTIDAD, MOTIVO, RESPONSABLE_ID)
            VALUES (:loteId, :tipo, :cantidad, :motivo, :responsableId)";

        EjecutarComando(sql, cmd =>
        {
            cmd.Parameters.Add("loteId", OracleDbType.Int32).Value = ajuste.LoteId;
            cmd.Parameters.Add("tipo", OracleDbType.Varchar2).Value = ajuste.Tipo.ToString();
            cmd.Parameters.Add("cantidad", OracleDbType.Int32).Value = ajuste.Cantidad;
            cmd.Parameters.Add("motivo", OracleDbType.Varchar2).Value = ajuste.Motivo;
            cmd.Parameters.Add("responsableId", OracleDbType.Int32).Value = ajuste.ResponsableId;
        });
    }

    public IEnumerable<AjusteInventario> ObtenerPorLote(int loteId)
    {
        const string sql = @"
            SELECT ID, LOTE_ID, TIPO, CANTIDAD, MOTIVO, FECHA_AJUSTE, RESPONSABLE_ID,
                   ACTIVO, FECHA_CREACION
            FROM AJUSTE_INVENTARIO 
            WHERE LOTE_ID = :loteId
            ORDER BY FECHA_AJUSTE DESC";

        return EjecutarConsultaLista(sql,
            cmd => cmd.Parameters.Add("loteId", OracleDbType.Int32).Value = loteId,
            MapearAjuste);
    }

    public IEnumerable<AjusteInventario> ObtenerPorResponsable(int usuarioId)
    {
        const string sql = @"
            SELECT ID, LOTE_ID, TIPO, CANTIDAD, MOTIVO, FECHA_AJUSTE, RESPONSABLE_ID,
                   ACTIVO, FECHA_CREACION
            FROM AJUSTE_INVENTARIO 
            WHERE RESPONSABLE_ID = :userId
            ORDER BY FECHA_AJUSTE DESC";

        return EjecutarConsultaLista(sql,
            cmd => cmd.Parameters.Add("userId", OracleDbType.Int32).Value = usuarioId,
            MapearAjuste);
    }

    public IEnumerable<AjusteInventario> ObtenerPorRangoFechas(DateTime desde, DateTime hasta)
    {
        const string sql = @"
            SELECT ID, LOTE_ID, TIPO, CANTIDAD, MOTIVO, FECHA_AJUSTE, RESPONSABLE_ID,
                   ACTIVO, FECHA_CREACION
            FROM AJUSTE_INVENTARIO 
            WHERE FECHA_AJUSTE BETWEEN :desde AND :hasta
            ORDER BY FECHA_AJUSTE DESC";

        return EjecutarConsultaLista(sql, cmd =>
        {
            cmd.Parameters.Add("desde", OracleDbType.Date).Value = desde;
            cmd.Parameters.Add("hasta", OracleDbType.Date).Value = hasta;
        }, MapearAjuste);
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