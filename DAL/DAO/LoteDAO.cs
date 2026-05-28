using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;
using System.Data;

namespace DAL.DAO;

public class LoteDAO : BaseDAO, ILoteDAO
{
    public LoteDAO(IOracleConnectionFactory conexionFactory) : base(conexionFactory) { }

    public Lote? ObtenerPorId(int id)
    {
        Lote? resultado = null;
        EjecutarCursor("PKG_PHARMASMART_FEFO.OBTENER_LOTE_POR_ID",
            cmd => cmd.Parameters.Add("p_id_lote", OracleDbType.Int32).Value = id,
            reader => { if (reader.Read()) resultado = MapearLote(reader); });
        return resultado;
    }

    public List<Lote> ObtenerPorProducto(int productoId)
    {
        var lista = new List<Lote>();
        EjecutarCursor("PKG_PHARMASMART_FEFO.OBTENER_LOTES_POR_PRODUCTO",
            cmd => cmd.Parameters.Add("p_id_producto", OracleDbType.Int32).Value = productoId,
            reader => { while (reader.Read()) lista.Add(MapearLote(reader)); });
        return lista;
    }

    public List<Lote> ObtenerTodosActivos()
    {
        var lista = new List<Lote>();
        EjecutarCursor("PKG_PHARMASMART_FEFO.OBTENER_LOTES_ACTIVOS",
            cmd => { },
            reader => { while (reader.Read()) lista.Add(MapearLote(reader)); });
        return lista;
    }

    public void Insertar(Lote lote)
    {
        EjecutarProcedimiento("PKG_PHARMASMART_FEFO.INSERTAR_LOTE", cmd =>
        {
            cmd.Parameters.Add("p_codigo_lote", OracleDbType.Varchar2).Value = lote.CodigoLote;
            cmd.Parameters.Add("p_id_producto", OracleDbType.Int32).Value = lote.IdProducto;
            cmd.Parameters.Add("p_fecha_fabricacion", OracleDbType.Date).Value = lote.FechaFabricacion;
            cmd.Parameters.Add("p_fecha_vencimiento", OracleDbType.Date).Value = lote.FechaVencimiento;
            cmd.Parameters.Add("p_precio_compra", OracleDbType.Decimal).Value = lote.PrecioCompra;
            cmd.Parameters.Add("p_precio_venta", OracleDbType.Decimal).Value = lote.PrecioVenta;
            cmd.Parameters.Add("p_cantidad_inicial", OracleDbType.Int32).Value = lote.CantidadInicial;
        });
    }

    public void Actualizar(Lote lote)
    {
        EjecutarProcedimiento("PKG_PHARMASMART_FEFO.ACTUALIZAR_LOTE", cmd =>
        {
            cmd.Parameters.Add("p_id_lote", OracleDbType.Int32).Value = lote.IdLote;
            cmd.Parameters.Add("p_cantidad_actual", OracleDbType.Int32).Value = lote.CantidadActual;
            cmd.Parameters.Add("p_estado", OracleDbType.Char).Value = lote.Estado;
        });
    }

    public void ActualizarStock(int loteId, int nuevaCantidad, EstadoLote estado)
    {
        char estadoChar = estado switch
        {
            EstadoLote.Activo => 'A',
            EstadoLote.Agotado => 'B',
            EstadoLote.Vencido => 'V',
            EstadoLote.EnCuarentena => 'C',
            _ => 'A'
        };

        EjecutarProcedimiento("PKG_PHARMASMART_FEFO.ACTUALIZAR_STOCK", cmd =>
        {
            cmd.Parameters.Add("p_id_lote", OracleDbType.Int32).Value = loteId;
            cmd.Parameters.Add("p_cantidad", OracleDbType.Int32).Value = nuevaCantidad;
            cmd.Parameters.Add("p_estado", OracleDbType.Char).Value = estadoChar;
        });
    }

    public Lote? SeleccionarLoteFEFO(int productoId)
    {
        Lote? resultado = null;
        EjecutarCursor("PKG_PHARMASMART_FEFO.SELECCIONAR_FEFO",
            cmd => cmd.Parameters.Add("p_id_producto", OracleDbType.Int32).Value = productoId,
            reader => { if (reader.Read()) resultado = MapearLote(reader); });
        return resultado;
    }

    private Lote MapearLote(OracleDataReader reader)
    {
        int cantidadInicial = reader.GetInt32(reader.GetOrdinal("CANTIDAD_INICIAL"));
        var lote = new Lote(cantidadInicial);

        typeof(Lote).GetProperty(nameof(Lote.IdLote))?.SetValue(lote, reader.GetInt32(reader.GetOrdinal("ID_LOTE")));
        typeof(Lote).GetProperty(nameof(Lote.CodigoLote))?.SetValue(lote, LeerString(reader, "CODIGO_LOTE"));
        typeof(Lote).GetProperty(nameof(Lote.IdProducto))?.SetValue(lote, reader.GetInt32(reader.GetOrdinal("ID_PRODUCTO")));
        typeof(Lote).GetProperty(nameof(Lote.FechaFabricacion))?.SetValue(lote, reader.GetDateTime(reader.GetOrdinal("FECHA_FABRICACION")));
        typeof(Lote).GetProperty(nameof(Lote.FechaVencimiento))?.SetValue(lote, reader.GetDateTime(reader.GetOrdinal("FECHA_VENCIMIENTO")));
        typeof(Lote).GetProperty(nameof(Lote.PrecioCompra))?.SetValue(lote, reader.GetDecimal(reader.GetOrdinal("PRECIO_COMPRA")));
        typeof(Lote).GetProperty(nameof(Lote.PrecioVenta))?.SetValue(lote, reader.GetDecimal(reader.GetOrdinal("PRECIO_VENTA")));
        typeof(Lote).GetProperty(nameof(Lote.Estado))?.SetValue(lote, LeerChar(reader, "ESTADO"));
        typeof(Lote).GetProperty("CantidadActual")?.SetValue(lote, reader.GetInt32(reader.GetOrdinal("CANTIDAD_ACTUAL")));

        return lote;
    }
}