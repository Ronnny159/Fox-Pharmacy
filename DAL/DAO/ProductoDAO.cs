using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;

namespace DAL.DAO;

public class ProductoDAO : BaseDAO, IProductoDAO
{
    public ProductoDAO(IOracleConnectionFactory conexionFactory) : base(conexionFactory) { }

    public Producto? ObtenerPorCodigo(string codigo)
    {
        Producto? resultado = null;
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_PRODUCTO_POR_CODIGO",
            cmd => cmd.Parameters.Add("p_codigo", OracleDbType.Varchar2).Value = codigo,
            reader => { if (reader.Read()) resultado = MapearProducto(reader); });
        return resultado;
    }

    public Producto? ObtenerPorId(int id)
    {
        Producto? resultado = null;
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_PRODUCTO_POR_ID",
            cmd => cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = id,
            reader => { if (reader.Read()) resultado = MapearProducto(reader); });
        return resultado;
    }

    public Producto? ObtenerPorNombre(string nombre)
    {
        Producto? resultado = null;
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_PRODUCTO_POR_NOMBRE",
            cmd => cmd.Parameters.Add("p_nombre", OracleDbType.Varchar2).Value = nombre,
            reader => { if (reader.Read()) resultado = MapearProducto(reader); });
        return resultado;
    }

    public List<Producto> ObtenerTodos()
    {
        var lista = new List<Producto>();
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_TODOS_PRODUCTOS",
            cmd => { },
            reader => { while (reader.Read()) lista.Add(MapearProducto(reader)); });
        return lista;
    }

    public void Insertar(Producto producto)
    {
        EjecutarProcedimiento("PKG_PHARMASMART_CONFIG.INSERTAR_PRODUCTO", cmd =>
        {
            cmd.Parameters.Add("p_codigo", OracleDbType.Varchar2).Value = producto.Codigo;
            cmd.Parameters.Add("p_nombre", OracleDbType.Varchar2).Value = producto.Nombre;
            cmd.Parameters.Add("p_descripcion", OracleDbType.Varchar2).Value = producto.Descripcion ?? (object)DBNull.Value;
            cmd.Parameters.Add("p_es_controlado", OracleDbType.Int32).Value = producto.EsControlado ? 1 : 0;
            cmd.Parameters.Add("p_stock_minimo", OracleDbType.Int32).Value = producto.StockMinimo;
            cmd.Parameters.Add("p_descuento", OracleDbType.Decimal).Value = (object?)producto.DescuentoProximidadVencimiento ?? DBNull.Value;
        });
    }

    public void Actualizar(Producto producto)
    {
        EjecutarProcedimiento("PKG_PHARMASMART_CONFIG.ACTUALIZAR_PRODUCTO", cmd =>
        {
            cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = producto.Id;
            cmd.Parameters.Add("p_nombre", OracleDbType.Varchar2).Value = producto.Nombre;
            cmd.Parameters.Add("p_descripcion", OracleDbType.Varchar2).Value = producto.Descripcion ?? (object)DBNull.Value;
            cmd.Parameters.Add("p_es_controlado", OracleDbType.Int32).Value = producto.EsControlado ? 1 : 0;
            cmd.Parameters.Add("p_stock_minimo", OracleDbType.Int32).Value = producto.StockMinimo;
            cmd.Parameters.Add("p_descuento", OracleDbType.Decimal).Value = (object?)producto.DescuentoProximidadVencimiento ?? DBNull.Value;
        });
    }

    public void ActualizarDescuentoIndividual(int productoId, decimal? descuento)
    {
        EjecutarProcedimiento("PKG_PHARMASMART_CONFIG.ACTUALIZAR_DESCUENTO_INDIVIDUAL", cmd =>
        {
            cmd.Parameters.Add("p_producto_id", OracleDbType.Int32).Value = productoId;
            cmd.Parameters.Add("p_descuento", OracleDbType.Decimal).Value = (object?)descuento ?? DBNull.Value;
        });
    }

    public void Eliminar(int id)
    {
        EjecutarProcedimiento("PKG_PHARMASMART_CONFIG.ELIMINAR_PRODUCTO",
            cmd => cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = id);
    }

    private Producto MapearProducto(OracleDataReader reader)
    {
        return new Producto
        {
            Id = reader.GetInt32(reader.GetOrdinal("ID")),
            Codigo = LeerString(reader, "CODIGO"),
            Nombre = LeerString(reader, "NOMBRE"),
            Descripcion = LeerString(reader, "DESCRIPCION"),
            EsControlado = LeerBooleano(reader, "ES_CONTROLADO"),
            StockMinimo = reader.GetInt32(reader.GetOrdinal("STOCK_MINIMO")),
            DescuentoProximidadVencimiento = LeerDecimalNulo(reader, "DESCUENTO_PROXIMIDAD_VENCIMIENTO"),
            Activo = LeerBooleano(reader, "ACTIVO"),
            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
        };
    }
}