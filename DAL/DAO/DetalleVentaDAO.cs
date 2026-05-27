using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;

namespace DAL.DAO;

public class DetalleVentaDAO : BaseDAO, IDetalleVentaDAO
{
    public DetalleVentaDAO(IOracleConnectionFactory conexionFactory) : base(conexionFactory) { }

    public List<DetalleVenta> ObtenerPorVenta(int ventaId)
    {
        var lista = new List<DetalleVenta>();
        EjecutarCursor("PKG_PHARMASMART_VENTAS.OBTENER_DETALLES_VENTA",
            cmd => cmd.Parameters.Add("p_venta_id", OracleDbType.Int32).Value = ventaId,
            reader => { while (reader.Read()) lista.Add(MapearDetalleVenta(reader)); });
        return lista;
    }

    public List<DetalleVenta> ObtenerPorLote(int loteId)
    {
        var lista = new List<DetalleVenta>();
        EjecutarCursor("PKG_PHARMASMART_VENTAS.OBTENER_DETALLES_POR_LOTE",
            cmd => cmd.Parameters.Add("p_lote_id", OracleDbType.Int32).Value = loteId,
            reader => { while (reader.Read()) lista.Add(MapearDetalleVenta(reader)); });
        return lista;
    }

    public void Insertar(DetalleVenta detalle)
    {
        EjecutarProcedimiento("PKG_PHARMASMART_VENTAS.INSERTAR_DETALLE_VENTA", cmd =>
        {
            cmd.Parameters.Add("p_venta_id", OracleDbType.Int32).Value = detalle.VentaId;
            cmd.Parameters.Add("p_lote_id", OracleDbType.Int32).Value = detalle.LoteId;
            cmd.Parameters.Add("p_cantidad", OracleDbType.Int32).Value = detalle.Cantidad;
            cmd.Parameters.Add("p_precio_aplicado", OracleDbType.Decimal).Value = detalle.PrecioAplicado;
            cmd.Parameters.Add("p_descuento_unitario", OracleDbType.Decimal).Value = detalle.DescuentoUnitario;
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