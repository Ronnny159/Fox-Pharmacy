using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;
using System.Data;

namespace DAL.DAO;

public class VentaDAO : BaseDAO, IVentaDAO
{
    public VentaDAO(IOracleConnectionFactory conexionFactory) : base(conexionFactory) { }

    public Venta CrearVenta(Venta venta, List<DetalleVenta> detalles)
    {
        using var conexion = ObtenerConexion();
        using var transaccion = conexion.BeginTransaction();

        try
        {
            using var cmdVenta = new OracleCommand("PKG_PHARMASMART_VENTAS.CREAR_VENTA", conexion);
            cmdVenta.CommandType = CommandType.StoredProcedure;
            cmdVenta.Parameters.Add("p_usuario_id", OracleDbType.Int32).Value = venta.UsuarioId;
            cmdVenta.Parameters.Add("p_cliente_id", OracleDbType.Int32).Value = (object?)venta.ClienteId ?? DBNull.Value;
            cmdVenta.Parameters.Add("p_subtotal", OracleDbType.Decimal).Value = venta.Subtotal;
            cmdVenta.Parameters.Add("p_descuento_total", OracleDbType.Decimal).Value = venta.DescuentoTotal;
            cmdVenta.Parameters.Add("p_total", OracleDbType.Decimal).Value = venta.Total;
            var paramVentaId = new OracleParameter("p_venta_id", OracleDbType.Int32);
            paramVentaId.Direction = ParameterDirection.Output;
            cmdVenta.Parameters.Add(paramVentaId);
            var paramFactura = new OracleParameter("p_numero_factura", OracleDbType.Varchar2, 20);
            paramFactura.Direction = ParameterDirection.Output;
            cmdVenta.Parameters.Add(paramFactura);
            cmdVenta.ExecuteNonQuery();

            venta.Id = Convert.ToInt32(paramVentaId.Value.ToString());
            venta.NumeroFactura = paramFactura.Value!.ToString()!;

            foreach (var detalle in detalles)
            {
                detalle.VentaId = venta.Id;
                using var cmdDetalle = new OracleCommand("PKG_PHARMASMART_VENTAS.INSERTAR_DETALLE_VENTA", conexion);
                cmdDetalle.CommandType = CommandType.StoredProcedure;
                cmdDetalle.Parameters.Add("p_venta_id", OracleDbType.Int32).Value = detalle.VentaId;
                cmdDetalle.Parameters.Add("p_lote_id", OracleDbType.Int32).Value = detalle.LoteId;
                cmdDetalle.Parameters.Add("p_cantidad", OracleDbType.Int32).Value = detalle.Cantidad;
                cmdDetalle.Parameters.Add("p_precio_aplicado", OracleDbType.Decimal).Value = detalle.PrecioAplicado;
                cmdDetalle.Parameters.Add("p_descuento_unitario", OracleDbType.Decimal).Value = detalle.DescuentoUnitario;
                cmdDetalle.ExecuteNonQuery();
            }

            transaccion.Commit();
            return venta;
        }
        catch
        {
            transaccion.Rollback();
            throw;
        }
    }

    public Venta? ObtenerPorId(int id)
    {
        Venta? resultado = null;
        EjecutarCursor("PKG_PHARMASMART_VENTAS.OBTENER_VENTA_POR_ID",
            cmd => cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = id,
            reader => { if (reader.Read()) resultado = MapearVenta(reader); });
        return resultado;
    }

    public Venta? ObtenerPorNumeroFactura(string numeroFactura)
    {
        Venta? resultado = null;
        EjecutarCursor("PKG_PHARMASMART_VENTAS.OBTENER_VENTA_POR_FACTURA",
            cmd => cmd.Parameters.Add("p_numero_factura", OracleDbType.Varchar2).Value = numeroFactura,
            reader => { if (reader.Read()) resultado = MapearVenta(reader); });
        return resultado;
    }

    public List<Venta> ObtenerPorUsuario(int usuarioId)
    {
        var lista = new List<Venta>();
        EjecutarCursor("PKG_PHARMASMART_VENTAS.OBTENER_VENTAS_POR_USUARIO",
            cmd => cmd.Parameters.Add("p_usuario_id", OracleDbType.Int32).Value = usuarioId,
            reader => { while (reader.Read()) lista.Add(MapearVenta(reader)); });
        return lista;
    }

    public List<Venta> ObtenerPorRangoFechas(DateTime desde, DateTime hasta)
    {
        var lista = new List<Venta>();
        EjecutarCursor("PKG_PHARMASMART_VENTAS.OBTENER_VENTAS_POR_FECHAS",
            cmd =>
            {
                cmd.Parameters.Add("p_desde", OracleDbType.Date).Value = desde;
                cmd.Parameters.Add("p_hasta", OracleDbType.Date).Value = hasta;
            },
            reader => { while (reader.Read()) lista.Add(MapearVenta(reader)); });
        return lista;
    }

    public void AnularVenta(int ventaId, int usuarioId)
    {
        EjecutarProcedimiento("PKG_PHARMASMART_VENTAS.ANULAR_VENTA", cmd =>
        {
            cmd.Parameters.Add("p_venta_id", OracleDbType.Int32).Value = ventaId;
            cmd.Parameters.Add("p_usuario_id", OracleDbType.Int32).Value = usuarioId;
        });
    }

    private Venta MapearVenta(OracleDataReader reader)
    {
        return new Venta
        {
            Id = reader.GetInt32(reader.GetOrdinal("ID")),
            NumeroFactura = LeerString(reader, "NUMERO_FACTURA"),
            FechaVenta = reader.GetDateTime(reader.GetOrdinal("FECHA_VENTA")),
            UsuarioId = reader.GetInt32(reader.GetOrdinal("USUARIO_ID")),
            ClienteId = LeerIntNulo(reader, "CLIENTE_ID"),
            Subtotal = reader.GetDecimal(reader.GetOrdinal("SUBTOTAL")),
            DescuentoTotal = reader.GetDecimal(reader.GetOrdinal("DESCUENTO_TOTAL")),
            Total = reader.GetDecimal(reader.GetOrdinal("TOTAL")),
            Anulada = LeerBooleano(reader, "ANULADA"),
            FechaAnulacion = LeerFechaNulo(reader, "FECHA_ANULACION"),
            AnuladaPor = LeerIntNulo(reader, "ANULADA_POR"),
            Activo = LeerBooleano(reader, "ACTIVO"),
            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
        };
    }
}