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
            cmd => cmd.Parameters.Add("p_id_venta", OracleDbType.Int32).Value = ventaId,
            reader => { while (reader.Read()) lista.Add(MapearDetalleVenta(reader)); });
        return lista;
    }

    public List<DetalleVenta> ObtenerPorLote(int loteId)
    {
        var lista = new List<DetalleVenta>();
        EjecutarCursor("PKG_PHARMASMART_VENTAS.OBTENER_DETALLES_POR_LOTE",
            cmd => cmd.Parameters.Add("p_id_lote", OracleDbType.Int32).Value = loteId,
            reader => { while (reader.Read()) lista.Add(MapearDetalleVenta(reader)); });
        return lista;
    }

    public void Insertar(DetalleVenta detalle)
    {
        EjecutarProcedimiento("PKG_PHARMASMART_VENTAS.INSERTAR_DETALLE", cmd =>
        {
            cmd.Parameters.Add("p_id_venta", OracleDbType.Int32).Value = detalle.IdVenta;
            cmd.Parameters.Add("p_id_producto", OracleDbType.Int32).Value = detalle.IdProducto;
            cmd.Parameters.Add("p_id_lote", OracleDbType.Int32).Value = detalle.IdLote;
            cmd.Parameters.Add("p_cantidad", OracleDbType.Int32).Value = detalle.Cantidad;
            cmd.Parameters.Add("p_precio_aplicado", OracleDbType.Decimal).Value = detalle.PrecioAplicado;
            cmd.Parameters.Add("p_descuento_unitario", OracleDbType.Decimal).Value = detalle.DescuentoUnitario;
        });
    }

    private DetalleVenta MapearDetalleVenta(OracleDataReader reader)
    {
        return new DetalleVenta
        {
            IdDetalle = reader.GetInt32(reader.GetOrdinal("ID_DETALLE")),
            IdVenta = reader.GetInt32(reader.GetOrdinal("ID_VENTA")),
            IdProducto = reader.GetInt32(reader.GetOrdinal("ID_PRODUCTO")),
            IdLote = reader.GetInt32(reader.GetOrdinal("ID_LOTE")),
            Cantidad = reader.GetInt32(reader.GetOrdinal("CANTIDAD")),
            PrecioAplicado = reader.GetDecimal(reader.GetOrdinal("PRECIO_APLICADO")),
            DescuentoUnitario = reader.GetDecimal(reader.GetOrdinal("DESCUENTO_UNITARIO"))
        };
    }
}