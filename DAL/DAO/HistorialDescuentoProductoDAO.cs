using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;
using System.Data;

namespace DAL.DAO;

public class HistorialDescuentoProductoDAO : BaseDAO, IHistorialDescuentoProductoDAO
{
    public HistorialDescuentoProductoDAO(IOracleConnectionFactory conexionFactory) : base(conexionFactory) { }

    public void Insertar(HistorialDescuentoProducto historial)
    {
        EjecutarProcedimiento("PKG_PHARMASMART_ALERTAS.INSERTAR_HISTORIAL_DESCUENTO", cmd =>
        {
            cmd.Parameters.Add("p_producto_id", OracleDbType.Int32).Value = historial.ProductoId;
            cmd.Parameters.Add("p_codigo_producto", OracleDbType.Varchar2).Value = historial.CodigoProducto;
            cmd.Parameters.Add("p_nombre_producto", OracleDbType.Varchar2).Value = historial.NombreProducto;
            cmd.Parameters.Add("p_descuento_anterior", OracleDbType.Decimal).Value = (object?)historial.DescuentoAnterior ?? DBNull.Value;
            cmd.Parameters.Add("p_descuento_nuevo", OracleDbType.Decimal).Value = (object?)historial.DescuentoNuevo ?? DBNull.Value;
            cmd.Parameters.Add("p_accion", OracleDbType.Varchar2).Value = historial.Accion;
            cmd.Parameters.Add("p_motivo", OracleDbType.Varchar2).Value = historial.Motivo;
            cmd.Parameters.Add("p_modificado_por_id", OracleDbType.Int32).Value = historial.ModificadoPorId;
            cmd.Parameters.Add("p_direccion_ip", OracleDbType.Varchar2).Value = historial.DireccionIP ?? (object)DBNull.Value;
        });
    }

    public List<HistorialDescuentoProducto> ObtenerPorProducto(int productoId)
    {
        var lista = new List<HistorialDescuentoProducto>();
        EjecutarCursor("PKG_PHARMASMART_ALERTAS.OBTENER_HISTORIAL_DESCUENTO_POR_PRODUCTO",
            cmd => cmd.Parameters.Add("p_producto_id", OracleDbType.Int32).Value = productoId,
            reader => { while (reader.Read()) lista.Add(MapearHistorial(reader)); });
        return lista;
    }

    public List<HistorialDescuentoProducto> ObtenerTodos()
    {
        var lista = new List<HistorialDescuentoProducto>();
        EjecutarCursor("PKG_PHARMASMART_ALERTAS.OBTENER_TODO_HISTORIAL_DESCUENTO",
            cmd => { },
            reader => { while (reader.Read()) lista.Add(MapearHistorial(reader)); });
        return lista;
    }

    private HistorialDescuentoProducto MapearHistorial(OracleDataReader reader)
    {
        return new HistorialDescuentoProducto
        {
            Id = reader.GetInt32(reader.GetOrdinal("ID")),
            ProductoId = reader.GetInt32(reader.GetOrdinal("PRODUCTO_ID")),
            CodigoProducto = LeerString(reader, "CODIGO_PRODUCTO"),
            NombreProducto = LeerString(reader, "NOMBRE_PRODUCTO"),
            DescuentoAnterior = LeerDecimalNulo(reader, "DESCUENTO_ANTERIOR"),
            DescuentoNuevo = LeerDecimalNulo(reader, "DESCUENTO_NUEVO"),
            Accion = LeerString(reader, "ACCION"),
            Motivo = LeerString(reader, "MOTIVO"),
            FechaCambio = reader.GetDateTime(reader.GetOrdinal("FECHA_CAMBIO")),
            ModificadoPorId = reader.GetInt32(reader.GetOrdinal("MODIFICADO_POR_ID")),
            DireccionIP = LeerString(reader, "DIRECCION_IP"),
            Activo = LeerBooleano(reader, "ACTIVO"),
            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
        };
    }
}