using System;
using System.Collections.Generic;
using Entity;
using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using System.Data;

namespace DAL.Repositories;

public class VentaRepository : BaseRepository, IVentaRepository
{
    private readonly IDetalleVentaRepository _detalleVentaRepo;

    public VentaRepository(IOracleConnectionFactory conexionFactory, IDetalleVentaRepository detalleVentaRepo)
        : base(conexionFactory)
    {
        _detalleVentaRepo = detalleVentaRepo;
    }

    public Venta CrearVenta(Venta venta, List<DetalleVenta> detalles)
    {
        using var conexion = ObtenerConexion();
        using var transaccion = conexion.BeginTransaction();

        try
        {
            // Crear cabecera de venta
            using var cmdVenta = new OracleCommand("SP_CREAR_VENTA", conexion);
            cmdVenta.CommandType = CommandType.StoredProcedure;
            cmdVenta.Parameters.Add("p_usuario_id", OracleDbType.Int32).Value = venta.UsuarioId;
            cmdVenta.Parameters.Add("p_cliente_id", OracleDbType.Int32).Value =
                (object?)venta.ClienteId ?? DBNull.Value;
            cmdVenta.Parameters.Add("p_subtotal", OracleDbType.Decimal).Value = venta.Subtotal;
            cmdVenta.Parameters.Add("p_descuento_total", OracleDbType.Decimal).Value = venta.DescuentoTotal;
            cmdVenta.Parameters.Add("p_total", OracleDbType.Decimal).Value = venta.Total;
            cmdVenta.Parameters.Add("p_venta_id", OracleDbType.Int32, ParameterDirection.Output);
            cmdVenta.Parameters.Add("p_numero_factura", OracleDbType.Varchar2, 20, null, ParameterDirection.Output);

            cmdVenta.ExecuteNonQuery();

            venta.Id = Convert.ToInt32(cmdVenta.Parameters["p_venta_id"].Value.ToString());
            venta.NumeroFactura = cmdVenta.Parameters["p_numero_factura"].Value!.ToString()!;

            // Insertar cada detalle
            foreach (var detalle in detalles)
            {
                detalle.VentaId = venta.Id;
                InsertarDetalleEnTransaccion(conexion, detalle);
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

    private void InsertarDetalleEnTransaccion(OracleConnection conexion, DetalleVenta detalle)
    {
        using var cmd = new OracleCommand("SP_INSERTAR_DETALLE_VENTA", conexion);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add("p_venta_id", OracleDbType.Int32).Value = detalle.VentaId;
        cmd.Parameters.Add("p_lote_id", OracleDbType.Int32).Value = detalle.LoteId;
        cmd.Parameters.Add("p_cantidad", OracleDbType.Int32).Value = detalle.Cantidad;
        cmd.Parameters.Add("p_precio_aplicado", OracleDbType.Decimal).Value = detalle.PrecioAplicado;
        cmd.Parameters.Add("p_descuento_unitario", OracleDbType.Decimal).Value = detalle.DescuentoUnitario;
        cmd.ExecuteNonQuery();
    }

    public Venta? ObtenerPorId(int id)
    {
        const string sql = @"
            SELECT ID, NUMERO_FACTURA, FECHA_VENTA, USUARIO_ID, CLIENTE_ID,
                   SUBTOTAL, DESCUENTO_TOTAL, TOTAL, ANULADA, FECHA_ANULACION, ANULADA_POR,
                   ACTIVO, FECHA_CREACION
            FROM VENTA 
            WHERE ID = :id";

        return EjecutarConsulta(sql,
            cmd => cmd.Parameters.Add("id", OracleDbType.Int32).Value = id,
            MapearVenta);
    }

    public Venta? ObtenerPorNumeroFactura(string numeroFactura)
    {
        const string sql = @"
            SELECT ID, NUMERO_FACTURA, FECHA_VENTA, USUARIO_ID, CLIENTE_ID,
                   SUBTOTAL, DESCUENTO_TOTAL, TOTAL, ANULADA, FECHA_ANULACION, ANULADA_POR,
                   ACTIVO, FECHA_CREACION
            FROM VENTA 
            WHERE NUMERO_FACTURA = :num";

        return EjecutarConsulta(sql,
            cmd => cmd.Parameters.Add("num", OracleDbType.Varchar2).Value = numeroFactura,
            MapearVenta);
    }

    public IEnumerable<Venta> ObtenerPorUsuario(int usuarioId)
    {
        const string sql = @"
            SELECT ID, NUMERO_FACTURA, FECHA_VENTA, USUARIO_ID, CLIENTE_ID,
                   SUBTOTAL, DESCUENTO_TOTAL, TOTAL, ANULADA, FECHA_ANULACION, ANULADA_POR,
                   ACTIVO, FECHA_CREACION
            FROM VENTA 
            WHERE USUARIO_ID = :userId AND ANULADA = 0
            ORDER BY FECHA_VENTA DESC";

        return EjecutarConsultaLista(sql,
            cmd => cmd.Parameters.Add("userId", OracleDbType.Int32).Value = usuarioId,
            MapearVenta);
    }

    public IEnumerable<Venta> ObtenerPorRangoFechas(DateTime desde, DateTime hasta)
    {
        const string sql = @"
            SELECT ID, NUMERO_FACTURA, FECHA_VENTA, USUARIO_ID, CLIENTE_ID,
                   SUBTOTAL, DESCUENTO_TOTAL, TOTAL, ANULADA, FECHA_ANULACION, ANULADA_POR,
                   ACTIVO, FECHA_CREACION
            FROM VENTA 
            WHERE FECHA_VENTA BETWEEN :desde AND :hasta AND ANULADA = 0
            ORDER BY FECHA_VENTA DESC";

        return EjecutarConsultaLista(sql, cmd =>
        {
            cmd.Parameters.Add("desde", OracleDbType.Date).Value = desde;
            cmd.Parameters.Add("hasta", OracleDbType.Date).Value = hasta;
        }, MapearVenta);
    }

    public void AnularVenta(int ventaId, int usuarioId)
    {
        EjecutarProcedimiento("SP_ANULAR_VENTA", cmd =>
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