using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;
using System;
using System.Collections.Generic;

namespace DAL.Repositories;

public class HistorialDescuentoProductoRepository : BaseRepository, IHistorialDescuentoProductoRepository
{
    public HistorialDescuentoProductoRepository(IOracleConnectionFactory conexionFactory)
        : base(conexionFactory) { }

    public void Insertar(HistorialDescuentoProducto historial)
    {
        const string sql = @"
            INSERT INTO HISTORIAL_DESCUENTO_PRODUCTO 
                (PRODUCTO_ID, CODIGO_PRODUCTO, NOMBRE_PRODUCTO, DESCUENTO_ANTERIOR, 
                 DESCUENTO_NUEVO, ACCION, MOTIVO, FECHA_CAMBIO, MODIFICADO_POR_ID, DIRECCION_IP)
            VALUES 
                (:prodId, :codigo, :nombre, :anterior, :nuevo, :accion, :motivo, :fecha, :usuarioId, :ip)";

        EjecutarComando(sql, cmd =>
        {
            cmd.Parameters.Add("prodId", OracleDbType.Int32).Value = historial.ProductoId;
            cmd.Parameters.Add("codigo", OracleDbType.Varchar2).Value = historial.CodigoProducto;
            cmd.Parameters.Add("nombre", OracleDbType.Varchar2).Value = historial.NombreProducto;
            cmd.Parameters.Add("anterior", OracleDbType.Decimal).Value = (object?)historial.DescuentoAnterior ?? DBNull.Value;
            cmd.Parameters.Add("nuevo", OracleDbType.Decimal).Value = (object?)historial.DescuentoNuevo ?? DBNull.Value;
            cmd.Parameters.Add("accion", OracleDbType.Varchar2).Value = historial.Accion;
            cmd.Parameters.Add("motivo", OracleDbType.Varchar2).Value = historial.Motivo;
            cmd.Parameters.Add("fecha", OracleDbType.Date).Value = historial.FechaCambio;
            cmd.Parameters.Add("usuarioId", OracleDbType.Int32).Value = historial.ModificadoPorId;
            cmd.Parameters.Add("ip", OracleDbType.Varchar2).Value = (object?)historial.DireccionIP ?? DBNull.Value;
        });
    }

    public List<HistorialDescuentoProducto> ObtenerPorProducto(int productoId)
    {
        const string sql = @"
            SELECT ID, PRODUCTO_ID, CODIGO_PRODUCTO, NOMBRE_PRODUCTO, DESCUENTO_ANTERIOR, 
                   DESCUENTO_NUEVO, ACCION, MOTIVO, FECHA_CAMBIO, MODIFICADO_POR_ID, DIRECCION_IP,
                   ACTIVO, FECHA_CREACION
            FROM HISTORIAL_DESCUENTO_PRODUCTO 
            WHERE PRODUCTO_ID = :prodId 
            ORDER BY FECHA_CAMBIO DESC";

        return EjecutarConsultaLista(sql,
            cmd => cmd.Parameters.Add("prodId", OracleDbType.Int32).Value = productoId,
            MapearHistorial);
    }

    public List<HistorialDescuentoProducto> ObtenerTodos()
    {
        const string sql = @"
            SELECT ID, PRODUCTO_ID, CODIGO_PRODUCTO, NOMBRE_PRODUCTO, DESCUENTO_ANTERIOR, 
                   DESCUENTO_NUEVO, ACCION, MOTIVO, FECHA_CAMBIO, MODIFICADO_POR_ID, DIRECCION_IP,
                   ACTIVO, FECHA_CREACION
            FROM HISTORIAL_DESCUENTO_PRODUCTO 
            ORDER BY FECHA_CAMBIO DESC";

        return EjecutarConsultaLista(sql, cmd => { }, MapearHistorial);
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