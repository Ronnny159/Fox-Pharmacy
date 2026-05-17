using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;
using System;
using System.Collections.Generic;

namespace DAL.Repositories;

public class ParametroSistemaRepository : BaseRepository, IParametroSistemaRepository
{
    public ParametroSistemaRepository(IOracleConnectionFactory conexionFactory)
        : base(conexionFactory) { }

    public ParametroSistema? ObtenerPorClave(string clave)
    {
        const string sql = @"
            SELECT ID, CLAVE, VALOR, DESCRIPCION, ACTIVO, FECHA_CREACION
            FROM PARAMETRO_SISTEMA 
            WHERE CLAVE = :clave AND ACTIVO = 1";

        return EjecutarConsulta(sql,
            cmd => cmd.Parameters.Add("clave", OracleDbType.Varchar2).Value = clave,
            MapearParametro);
    }

    public IEnumerable<ParametroSistema> ObtenerTodos()
    {
        const string sql = @"
            SELECT ID, CLAVE, VALOR, DESCRIPCION, ACTIVO, FECHA_CREACION
            FROM PARAMETRO_SISTEMA 
            WHERE ACTIVO = 1
            ORDER BY CLAVE";

        return EjecutarConsultaLista(sql, cmd => { }, MapearParametro);
    }

    public void Insertar(ParametroSistema parametro)
    {
        const string sql = @"
            INSERT INTO PARAMETRO_SISTEMA (CLAVE, VALOR, DESCRIPCION)
            VALUES (:clave, :valor, :descripcion)";

        EjecutarComando(sql, cmd =>
        {
            cmd.Parameters.Add("clave", OracleDbType.Varchar2).Value = parametro.Clave;
            cmd.Parameters.Add("valor", OracleDbType.Varchar2).Value = parametro.Valor;
            cmd.Parameters.Add("descripcion", OracleDbType.Varchar2).Value = parametro.Descripcion ?? (object)DBNull.Value;
        });
    }

    public void Actualizar(ParametroSistema parametro)
    {
        const string sql = @"
            UPDATE PARAMETRO_SISTEMA 
            SET VALOR = :valor, DESCRIPCION = :descripcion
            WHERE CLAVE = :clave";

        EjecutarComando(sql, cmd =>
        {
            cmd.Parameters.Add("clave", OracleDbType.Varchar2).Value = parametro.Clave;
            cmd.Parameters.Add("valor", OracleDbType.Varchar2).Value = parametro.Valor;
            cmd.Parameters.Add("descripcion", OracleDbType.Varchar2).Value = parametro.Descripcion ?? (object)DBNull.Value;
        });
    }

    private ParametroSistema MapearParametro(OracleDataReader reader)
    {
        return new ParametroSistema
        {
            Id = reader.GetInt32(reader.GetOrdinal("ID")),
            Clave = LeerString(reader, "CLAVE"),
            Valor = LeerString(reader, "VALOR"),
            Descripcion = LeerString(reader, "DESCRIPCION"),
            Activo = LeerBooleano(reader, "ACTIVO"),
            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
        };
    }
}