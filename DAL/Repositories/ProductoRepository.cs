using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;

namespace DAL.Repositories;

public class ProductoRepository : BaseRepository, IProductoRepository
{
    public ProductoRepository(IOracleConnectionFactory conexionFactory)
        : base(conexionFactory) { }

    public Producto? ObtenerPorCodigo(string codigo)
    {
        const string sql = @"
            SELECT ID, CODIGO, NOMBRE, DESCRIPCION, ES_CONTROLADO, STOCK_MINIMO, 
                   DESCUENTO_PROXIMIDAD_VENCIMIENTO, ACTIVO, FECHA_CREACION
            FROM PRODUCTO 
            WHERE CODIGO = :codigo AND ACTIVO = 1";

        return EjecutarConsulta(sql,
            cmd => cmd.Parameters.Add("codigo", OracleDbType.Varchar2).Value = codigo,
            MapearProducto);
    }

    public Producto? ObtenerPorId(int id)
    {
        const string sql = @"
            SELECT ID, CODIGO, NOMBRE, DESCRIPCION, ES_CONTROLADO, STOCK_MINIMO, 
                   DESCUENTO_PROXIMIDAD_VENCIMIENTO, ACTIVO, FECHA_CREACION
            FROM PRODUCTO 
            WHERE ID = :id";

        return EjecutarConsulta(sql,
            cmd => cmd.Parameters.Add("id", OracleDbType.Int32).Value = id,
            MapearProducto);
    }

    public Producto? ObtenerPorNombre(string nombre)
    {
        const string sql = @"
            SELECT ID, CODIGO, NOMBRE, DESCRIPCION, ES_CONTROLADO, STOCK_MINIMO, 
                   DESCUENTO_PROXIMIDAD_VENCIMIENTO, ACTIVO, FECHA_CREACION
            FROM PRODUCTO 
            WHERE UPPER(NOMBRE) = UPPER(:nombre) AND ACTIVO = 1";

        return EjecutarConsulta(sql,
            cmd => cmd.Parameters.Add("nombre", OracleDbType.Varchar2).Value = nombre,
            MapearProducto);
    }

    public IEnumerable<Producto> ObtenerTodos()
    {
        const string sql = @"
            SELECT ID, CODIGO, NOMBRE, DESCRIPCION, ES_CONTROLADO, STOCK_MINIMO, 
                   DESCUENTO_PROXIMIDAD_VENCIMIENTO, ACTIVO, FECHA_CREACION
            FROM PRODUCTO 
            WHERE ACTIVO = 1
            ORDER BY NOMBRE";

        return EjecutarConsultaLista(sql, cmd => { }, MapearProducto);
    }

    public void Insertar(Producto producto)
    {
        const string sql = @"
            INSERT INTO PRODUCTO (CODIGO, NOMBRE, DESCRIPCION, ES_CONTROLADO, STOCK_MINIMO, DESCUENTO_PROXIMIDAD_VENCIMIENTO)
            VALUES (:codigo, :nombre, :descripcion, :esControlado, :stockMinimo, :descuento)";

        EjecutarComando(sql, cmd =>
        {
            cmd.Parameters.Add("codigo", OracleDbType.Varchar2).Value = producto.Codigo;
            cmd.Parameters.Add("nombre", OracleDbType.Varchar2).Value = producto.Nombre;
            cmd.Parameters.Add("descripcion", OracleDbType.Varchar2).Value = producto.Descripcion ?? (object)DBNull.Value;
            cmd.Parameters.Add("esControlado", OracleDbType.Int16).Value = producto.EsControlado ? 1 : 0;
            cmd.Parameters.Add("stockMinimo", OracleDbType.Int32).Value = producto.StockMinimo;
            cmd.Parameters.Add("descuento", OracleDbType.Decimal).Value =
                (object?)producto.DescuentoProximidadVencimiento ?? DBNull.Value;
        });
    }

    public void Actualizar(Producto producto)
    {
        const string sql = @"
            UPDATE PRODUCTO 
            SET NOMBRE = :nombre, 
                DESCRIPCION = :descripcion, 
                ES_CONTROLADO = :esControlado, 
                STOCK_MINIMO = :stockMinimo,
                DESCUENTO_PROXIMIDAD_VENCIMIENTO = :descuento
            WHERE ID = :id";

        EjecutarComando(sql, cmd =>
        {
            cmd.Parameters.Add("id", OracleDbType.Int32).Value = producto.Id;
            cmd.Parameters.Add("nombre", OracleDbType.Varchar2).Value = producto.Nombre;
            cmd.Parameters.Add("descripcion", OracleDbType.Varchar2).Value = producto.Descripcion ?? (object)DBNull.Value;
            cmd.Parameters.Add("esControlado", OracleDbType.Int16).Value = producto.EsControlado ? 1 : 0;
            cmd.Parameters.Add("stockMinimo", OracleDbType.Int32).Value = producto.StockMinimo;
            cmd.Parameters.Add("descuento", OracleDbType.Decimal).Value =
                (object?)producto.DescuentoProximidadVencimiento ?? DBNull.Value;
        });
    }

    public void ActualizarDescuentoIndividual(int productoId, decimal? descuento)
    {
        const string sql = "UPDATE PRODUCTO SET DESCUENTO_PROXIMIDAD_VENCIMIENTO = :descuento WHERE ID = :id";

        EjecutarComando(sql, cmd =>
        {
            cmd.Parameters.Add("id", OracleDbType.Int32).Value = productoId;
            cmd.Parameters.Add("descuento", OracleDbType.Decimal).Value =
                (object?)descuento ?? DBNull.Value;
        });
    }

    public void Eliminar(int id)
    {
        const string sql = "UPDATE PRODUCTO SET ACTIVO = 0 WHERE ID = :id";
        EjecutarComando(sql, cmd => cmd.Parameters.Add("id", OracleDbType.Int32).Value = id);
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