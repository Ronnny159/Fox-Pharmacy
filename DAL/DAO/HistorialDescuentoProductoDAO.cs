using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;

namespace DAL.DAO;

public class HistorialDescuentoProductoDAO : BaseDAO, IHistorialDescuentoProductoDAO
{
    public HistorialDescuentoProductoDAO(IOracleConnectionFactory conexionFactory) : base(conexionFactory) { }

    public void Insertar(HistorialDescuentoProducto historial)
    {
        char accionChar = historial.Accion switch
        {
            "CREAR" => 'C',
            "MODIFICAR" => 'M',
            "ELIMINAR" => 'E',
            _ => 'M'
        };

        EjecutarProcedimiento("PKG_PHARMASMART_ALERTAS.INSERTAR_HISTORIAL_DESCUENTO", cmd =>
        {
            cmd.Parameters.Add("p_id_producto", OracleDbType.Int32).Value = historial.IdProducto;
            cmd.Parameters.Add("p_codigo_producto", OracleDbType.Varchar2).Value = historial.CodigoProducto;
            cmd.Parameters.Add("p_nombre_producto", OracleDbType.Varchar2).Value = historial.NombreProducto;
            cmd.Parameters.Add("p_descuento_anterior", OracleDbType.Decimal).Value = (object?)historial.DescuentoAnterior ?? DBNull.Value;
            cmd.Parameters.Add("p_descuento_nuevo", OracleDbType.Decimal).Value = (object?)historial.DescuentoNuevo ?? DBNull.Value;
            cmd.Parameters.Add("p_accion", OracleDbType.Char).Value = accionChar;
            cmd.Parameters.Add("p_motivo", OracleDbType.Varchar2).Value = historial.Motivo;
            cmd.Parameters.Add("p_id_usuario", OracleDbType.Int32).Value = historial.IdUsuario;
            cmd.Parameters.Add("p_ip", OracleDbType.Varchar2).Value = historial.DireccionIP ?? (object)DBNull.Value;
        });
    }

    public List<HistorialDescuentoProducto> ObtenerPorProducto(int productoId)
    {
        var lista = new List<HistorialDescuentoProducto>();
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_HISTORIAL_DESCUENTOS",
            cmd => cmd.Parameters.Add("p_id_producto", OracleDbType.Int32).Value = productoId,
            reader => { while (reader.Read()) lista.Add(MapearHistorial(reader)); });
        return lista;
    }

    public List<HistorialDescuentoProducto> ObtenerTodos()
    {
        var lista = new List<HistorialDescuentoProducto>();
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_TODO_HISTORIAL_DESCUENTO",
            cmd => { },
            reader => { while (reader.Read()) lista.Add(MapearHistorial(reader)); });
        return lista;
    }

    private HistorialDescuentoProducto MapearHistorial(OracleDataReader reader)
    {
        return new HistorialDescuentoProducto
        {
            IdHistorial = reader.GetInt32(reader.GetOrdinal("ID_HISTORIAL")),
            IdProducto = reader.GetInt32(reader.GetOrdinal("ID_PRODUCTO")),
            CodigoProducto = LeerString(reader, "CODIGO_PRODUCTO"),
            NombreProducto = LeerString(reader, "NOMBRE_PRODUCTO"),
            DescuentoAnterior = LeerDecimalNulo(reader, "DESCUENTO_ANTERIOR"),
            DescuentoNuevo = LeerDecimalNulo(reader, "DESCUENTO_NUEVO"),
            Accion = LeerChar(reader, "ACCION") switch { 'C' => "CREAR", 'M' => "MODIFICAR", 'E' => "ELIMINAR", _ => "MODIFICAR" },
            Motivo = LeerString(reader, "MOTIVO"),
            FechaCambio = reader.GetDateTime(reader.GetOrdinal("FECHA_CAMBIO")),
            IdUsuario = reader.GetInt32(reader.GetOrdinal("ID_USUARIO")),
            DireccionIP = LeerString(reader, "DIRECCION_IP")
        };
    }
}