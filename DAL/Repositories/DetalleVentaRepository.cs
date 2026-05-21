using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;

namespace DAL.Repositories;

public class DetalleVentaRepository : BaseRepository, IDetalleVentaRepository
{
    public DetalleVentaRepository(IOracleConnectionFactory conexionFactory)
        : base(conexionFactory) { }

    public List<DetalleVenta> ObtenerPorVenta(int ventaId)
    {
        const string sql = @"
            SELECT ID, VENTA_ID, LOTE_ID, CANTIDAD, PRECIO_APLICADO, DESCUENTO_UNITARIO,
                   ACTIVO, FECHA_CREACION
            FROM DETALLE_VENTA 
            WHERE VENTA_ID = :ventaId";

        return EjecutarConsultaLista(sql,
            cmd => cmd.Parameters.Add("ventaId", OracleDbType.Int32).Value = ventaId,
            MapearDetalleVenta);
    }

    public List<DetalleVenta> ObtenerPorLote(int loteId)
    {
        const string sql = @"
            SELECT ID, VENTA_ID, LOTE_ID, CANTIDAD, PRECIO_APLICADO, DESCUENTO_UNITARIO,
                   ACTIVO, FECHA_CREACION
            FROM DETALLE_VENTA 
            WHERE LOTE_ID = :loteId";

        return EjecutarConsultaLista(sql,
            cmd => cmd.Parameters.Add("loteId", OracleDbType.Int32).Value = loteId,
            MapearDetalleVenta);
    }

    public void Insertar(DetalleVenta detalle)
    {
        const string sql = @"
            INSERT INTO DETALLE_VENTA (VENTA_ID, LOTE_ID, CANTIDAD, PRECIO_APLICADO, DESCUENTO_UNITARIO)
            VALUES (:ventaId, :loteId, :cantidad, :precioAplicado, :descuentoUnitario)";

        EjecutarComando(sql, cmd =>
        {
            cmd.Parameters.Add("ventaId", OracleDbType.Int32).Value = detalle.VentaId;
            cmd.Parameters.Add("loteId", OracleDbType.Int32).Value = detalle.LoteId;
            cmd.Parameters.Add("cantidad", OracleDbType.Int32).Value = detalle.Cantidad;
            cmd.Parameters.Add("precioAplicado", OracleDbType.Decimal).Value = detalle.PrecioAplicado;
            cmd.Parameters.Add("descuentoUnitario", OracleDbType.Decimal).Value = detalle.DescuentoUnitario;
        });
    }

    private DetalleVenta MapearDetalleVenta(OracleDataReader reader)
    {
        return new DetalleVenta
        {
            Id = reader.GetInt32(reader.GetOrdinal("ID")),
            VentaId = reader.GetInt32(reader.GetOrdinal("VENTA_ID")),
            LoteId = reader.GetInt32(reader.GetOrdinal("LOTE_ID")),
            Cantidad = reader.GetInt32(reader.GetOrdinal("CANTIDAD")),
            PrecioAplicado = reader.GetDecimal(reader.GetOrdinal("PRECIO_APLICADO")),
            DescuentoUnitario = reader.GetDecimal(reader.GetOrdinal("DESCUENTO_UNITARIO")),
            Activo = LeerBooleano(reader, "ACTIVO"),
            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
        };
    }
}