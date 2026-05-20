using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;
using System;
using System.Collections.Generic;

namespace DAL.Repositories;

public class HistorialParametroRepository : BaseRepository, IHistorialParametroRepository
{
    public HistorialParametroRepository(IOracleConnectionFactory conexionFactory)
        : base(conexionFactory) { }

    public void Insertar(HistorialParametro historial)
    {
        const string sql = @"
            INSERT INTO HISTORIAL_PARAMETRO (CLAVE_PARAMETRO, VALOR_ANTERIOR, VALOR_NUEVO, MOTIVO, FECHA_CAMBIO, MODIFICADO_POR_ID, DIRECCION_IP)
            VALUES (:clave, :anterior, :nuevo, :motivo, :fecha, :usuarioId, :ip)";

        EjecutarComando(sql, cmd =>
        {
            cmd.Parameters.Add("clave", OracleDbType.Varchar2).Value = historial.ClaveParametro;
            cmd.Parameters.Add("anterior", OracleDbType.Varchar2).Value = historial.ValorAnterior;
            cmd.Parameters.Add("nuevo", OracleDbType.Varchar2).Value = historial.ValorNuevo;
            cmd.Parameters.Add("motivo", OracleDbType.Varchar2).Value = historial.Motivo;
            cmd.Parameters.Add("fecha", OracleDbType.Date).Value = historial.FechaCambio;
            cmd.Parameters.Add("usuarioId", OracleDbType.Int32).Value = historial.ModificadoPorId;
            cmd.Parameters.Add("ip", OracleDbType.Varchar2).Value = (object?)historial.DireccionIP ?? DBNull.Value;
        });
    }

    public List<HistorialParametro> ObtenerPorParametro(string clave)
    {
        const string sql = @"
            SELECT ID, CLAVE_PARAMETRO, VALOR_ANTERIOR, VALOR_NUEVO, MOTIVO, FECHA_CAMBIO,
                   MODIFICADO_POR_ID, DIRECCION_IP, ACTIVO, FECHA_CREACION
            FROM HISTORIAL_PARAMETRO 
            WHERE CLAVE_PARAMETRO = :clave 
            ORDER BY FECHA_CAMBIO DESC";

        return EjecutarConsultaLista(sql,
            cmd => cmd.Parameters.Add("clave", OracleDbType.Varchar2).Value = clave,
            MapearHistorial);
    }

    public List<HistorialParametro> ObtenerPorUsuario(int usuarioId)
    {
        const string sql = @"
            SELECT ID, CLAVE_PARAMETRO, VALOR_ANTERIOR, VALOR_NUEVO, MOTIVO, FECHA_CAMBIO,
                   MODIFICADO_POR_ID, DIRECCION_IP, ACTIVO, FECHA_CREACION
            FROM HISTORIAL_PARAMETRO 
            WHERE MODIFICADO_POR_ID = :usuarioId 
            ORDER BY FECHA_CAMBIO DESC";

        return EjecutarConsultaLista(sql,
            cmd => cmd.Parameters.Add("usuarioId", OracleDbType.Int32).Value = usuarioId,
            MapearHistorial);
    }

    public List<HistorialParametro> ObtenerPorRangoFechas(DateTime desde, DateTime hasta)
    {
        const string sql = @"
            SELECT ID, CLAVE_PARAMETRO, VALOR_ANTERIOR, VALOR_NUEVO, MOTIVO, FECHA_CAMBIO,
                   MODIFICADO_POR_ID, DIRECCION_IP, ACTIVO, FECHA_CREACION
            FROM HISTORIAL_PARAMETRO 
            WHERE FECHA_CAMBIO BETWEEN :desde AND :hasta 
            ORDER BY FECHA_CAMBIO DESC";

        return EjecutarConsultaLista(sql, cmd =>
        {
            cmd.Parameters.Add("desde", OracleDbType.Date).Value = desde;
            cmd.Parameters.Add("hasta", OracleDbType.Date).Value = hasta;
        }, MapearHistorial);
    }

    private HistorialParametro MapearHistorial(OracleDataReader reader)
    {
        return new HistorialParametro
        {
            Id = reader.GetInt32(reader.GetOrdinal("ID")),
            ClaveParametro = LeerString(reader, "CLAVE_PARAMETRO"),
            ValorAnterior = LeerString(reader, "VALOR_ANTERIOR"),
            ValorNuevo = LeerString(reader, "VALOR_NUEVO"),
            Motivo = LeerString(reader, "MOTIVO"),
            FechaCambio = reader.GetDateTime(reader.GetOrdinal("FECHA_CAMBIO")),
            ModificadoPorId = reader.GetInt32(reader.GetOrdinal("MODIFICADO_POR_ID")),
            DireccionIP = LeerString(reader, "DIRECCION_IP"),
            Activo = LeerBooleano(reader, "ACTIVO"),
            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
        };
    }
}