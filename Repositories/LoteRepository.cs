using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;
using System.Data;

namespace DAL.Repositories;

public class LoteRepository : BaseRepository, ILoteRepository
{
    public LoteRepository(IOracleConnectionFactory conexionFactory)
        : base(conexionFactory) { }

    public Lote? ObtenerPorId(int id)
    {
        const string sql = @"
            SELECT ID, CODIGO_LOTE, PRODUCTO_ID, FECHA_FABRICACION, FECHA_VENCIMIENTO,
                   PRECIO_COMPRA, PRECIO_VENTA, CANTIDAD_ACTUAL, CANTIDAD_INICIAL, ESTADO,
                   ACTIVO, FECHA_CREACION
            FROM LOTE 
            WHERE ID = :id";

        return EjecutarConsulta(sql,
            cmd => cmd.Parameters.Add("id", OracleDbType.Int32).Value = id,
            MapearLote);
    }

    public IEnumerable<Lote> ObtenerPorProducto(int productoId)
    {
        const string sql = @"
            SELECT ID, CODIGO_LOTE, PRODUCTO_ID, FECHA_FABRICACION, FECHA_VENCIMIENTO,
                   PRECIO_COMPRA, PRECIO_VENTA, CANTIDAD_ACTUAL, CANTIDAD_INICIAL, ESTADO,
                   ACTIVO, FECHA_CREACION
            FROM LOTE 
            WHERE PRODUCTO_ID = :productoId AND ACTIVO = 1
            ORDER BY FECHA_VENCIMIENTO ASC";

        return EjecutarConsultaLista(sql,
            cmd => cmd.Parameters.Add("productoId", OracleDbType.Int32).Value = productoId,
            MapearLote);
    }

    public IEnumerable<Lote> ObtenerTodosActivos()
    {
        const string sql = @"
            SELECT ID, CODIGO_LOTE, PRODUCTO_ID, FECHA_FABRICACION, FECHA_VENCIMIENTO,
                   PRECIO_COMPRA, PRECIO_VENTA, CANTIDAD_ACTUAL, CANTIDAD_INICIAL, ESTADO,
                   ACTIVO, FECHA_CREACION
            FROM LOTE 
            WHERE ACTIVO = 1 AND ESTADO = 1
            ORDER BY FECHA_VENCIMIENTO ASC";

        return EjecutarConsultaLista(sql, cmd => { }, MapearLote);
    }

    public void Insertar(Lote lote)
    {
        const string sql = @"
            INSERT INTO LOTE (CODIGO_LOTE, PRODUCTO_ID, FECHA_FABRICACION, FECHA_VENCIMIENTO,
                              PRECIO_COMPRA, PRECIO_VENTA, CANTIDAD_ACTUAL, CANTIDAD_INICIAL, ESTADO)
            VALUES (:codigoLote, :productoId, :fechaFabricacion, :fechaVencimiento,
                    :precioCompra, :precioVenta, :cantidadActual, :cantidadInicial, :estado)";

        EjecutarComando(sql, cmd =>
        {
            cmd.Parameters.Add("codigoLote", OracleDbType.Varchar2).Value = lote.CodigoLote;
            cmd.Parameters.Add("productoId", OracleDbType.Int32).Value = lote.ProductoId;
            cmd.Parameters.Add("fechaFabricacion", OracleDbType.Date).Value = lote.FechaFabricacion;
            cmd.Parameters.Add("fechaVencimiento", OracleDbType.Date).Value = lote.FechaVencimiento;
            cmd.Parameters.Add("precioCompra", OracleDbType.Decimal).Value = lote.PrecioCompra;
            cmd.Parameters.Add("precioVenta", OracleDbType.Decimal).Value = lote.PrecioVenta;
            cmd.Parameters.Add("cantidadActual", OracleDbType.Int32).Value = lote.CantidadActual;
            cmd.Parameters.Add("cantidadInicial", OracleDbType.Int32).Value = lote.CantidadInicial;
            cmd.Parameters.Add("estado", OracleDbType.Int32).Value = (int)lote.Estado;
        });
    }

    public void Actualizar(Lote lote)
    {
        const string sql = @"
            UPDATE LOTE 
            SET CANTIDAD_ACTUAL = :cantidadActual,
                ESTADO = :estado
            WHERE ID = :id";

        EjecutarComando(sql, cmd =>
        {
            cmd.Parameters.Add("id", OracleDbType.Int32).Value = lote.Id;
            cmd.Parameters.Add("cantidadActual", OracleDbType.Int32).Value = lote.CantidadActual;
            cmd.Parameters.Add("estado", OracleDbType.Int32).Value = (int)lote.Estado;
        });
    }

    public void ActualizarStock(int loteId, int nuevaCantidad, EstadoLote estado)
    {
        const string sql = @"
            UPDATE LOTE 
            SET CANTIDAD_ACTUAL = :cantidad, ESTADO = :estado 
            WHERE ID = :id";

        EjecutarComando(sql, cmd =>
        {
            cmd.Parameters.Add("id", OracleDbType.Int32).Value = loteId;
            cmd.Parameters.Add("cantidad", OracleDbType.Int32).Value = nuevaCantidad;
            cmd.Parameters.Add("estado", OracleDbType.Int32).Value = (int)estado;
        });
    }

    public Lote? SeleccionarLoteFEFO(int productoId)
    {
        const string sql = @"
            SELECT ID, CODIGO_LOTE, PRODUCTO_ID, FECHA_FABRICACION, FECHA_VENCIMIENTO,
                   PRECIO_COMPRA, PRECIO_VENTA, CANTIDAD_ACTUAL, CANTIDAD_INICIAL, ESTADO,
                   ACTIVO, FECHA_CREACION
            FROM LOTE
            WHERE PRODUCTO_ID = :productoId
              AND CANTIDAD_ACTUAL > 0
              AND ESTADO = 1
              AND FECHA_VENCIMIENTO > SYSDATE
            ORDER BY FECHA_VENCIMIENTO ASC, PRECIO_COMPRA ASC
            FETCH FIRST 1 ROWS ONLY";

        return EjecutarConsulta(sql,
            cmd => cmd.Parameters.Add("productoId", OracleDbType.Int32).Value = productoId,
            MapearLote);
    }

    private Lote MapearLote(OracleDataReader reader)
    {
        int cantidadInicial = reader.GetInt32(reader.GetOrdinal("CANTIDAD_INICIAL"));
        var lote = new Lote(cantidadInicial);

        // Usar reflexión para establecer propiedades con setter privado
        typeof(Lote).GetProperty(nameof(Lote.Id))?.SetValue(lote, reader.GetInt32(reader.GetOrdinal("ID")));
        typeof(Lote).GetProperty(nameof(Lote.CodigoLote))?.SetValue(lote, LeerString(reader, "CODIGO_LOTE"));
        typeof(Lote).GetProperty(nameof(Lote.ProductoId))?.SetValue(lote, reader.GetInt32(reader.GetOrdinal("PRODUCTO_ID")));
        typeof(Lote).GetProperty(nameof(Lote.FechaFabricacion))?.SetValue(lote, reader.GetDateTime(reader.GetOrdinal("FECHA_FABRICACION")));
        typeof(Lote).GetProperty(nameof(Lote.FechaVencimiento))?.SetValue(lote, reader.GetDateTime(reader.GetOrdinal("FECHA_VENCIMIENTO")));
        typeof(Lote).GetProperty(nameof(Lote.PrecioCompra))?.SetValue(lote, reader.GetDecimal(reader.GetOrdinal("PRECIO_COMPRA")));
        typeof(Lote).GetProperty(nameof(Lote.PrecioVenta))?.SetValue(lote, reader.GetDecimal(reader.GetOrdinal("PRECIO_VENTA")));
        typeof(Lote).GetProperty(nameof(Lote.Estado))?.SetValue(lote, (EstadoLote)reader.GetInt32(reader.GetOrdinal("ESTADO")));
        typeof(Lote).GetProperty(nameof(Lote.Activo))?.SetValue(lote, LeerBooleano(reader, "ACTIVO"));
        typeof(Lote).GetProperty(nameof(Lote.FechaCreacion))?.SetValue(lote, reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION")));

        // Establecer CantidadActual después de la creación
        typeof(Lote).GetProperty("CantidadActual")?.SetValue(lote, reader.GetInt32(reader.GetOrdinal("CANTIDAD_ACTUAL")));

        return lote;
    }
}