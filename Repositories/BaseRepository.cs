using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using System.Data;

namespace DAL.Repositories;

/// <summary>
/// Clase base abstracta para todos los repositorios.
/// Proporciona métodos comunes de ejecución de comandos y consultas.
/// Principio aplicado: DRY, Template Method (GRASP).
/// </summary>
public abstract class BaseRepository
{
    protected readonly IOracleConnectionFactory _conexionFactory;

    protected BaseRepository(IOracleConnectionFactory conexionFactory)
    {
        _conexionFactory = conexionFactory;
    }

    /// <summary>
    /// Obtiene una nueva conexión abierta a Oracle.
    /// </summary>
    protected OracleConnection ObtenerConexion()
    {
        return _conexionFactory.CrearConexion();
    }

    /// <summary>
    /// Ejecuta un comando SQL sin retorno de datos (INSERT, UPDATE, DELETE).
    /// </summary>
    protected void EjecutarComando(string sql, Action<OracleCommand> configurarParametros)
    {
        using var conexion = ObtenerConexion();
        using var comando = new OracleCommand(sql, conexion);
        configurarParametros(comando);
        comando.ExecuteNonQuery();
    }

    /// <summary>
    /// Ejecuta un procedimiento almacenado sin retorno de datos.
    /// </summary>
    protected void EjecutarProcedimiento(string nombreProcedimiento, Action<OracleCommand> configurarParametros)
    {
        using var conexion = ObtenerConexion();
        using var comando = new OracleCommand(nombreProcedimiento, conexion);
        comando.CommandType = CommandType.StoredProcedure;
        configurarParametros(comando);
        comando.ExecuteNonQuery();
    }

    /// <summary>
    /// Ejecuta una consulta que retorna un único registro.
    /// </summary>
    protected T? EjecutarConsulta<T>(string sql,
        Action<OracleCommand> configurarParametros,
        Func<OracleDataReader, T> mapearResultado)
    {
        using var conexion = ObtenerConexion();
        using var comando = new OracleCommand(sql, conexion);
        configurarParametros(comando);
        using var reader = comando.ExecuteReader();
        if (reader.Read())
            return mapearResultado(reader);
        return default;
    }

    /// <summary>
    /// Ejecuta una consulta que retorna múltiples registros.
    /// </summary>
    protected List<T> EjecutarConsultaLista<T>(string sql,
        Action<OracleCommand> configurarParametros,
        Func<OracleDataReader, T> mapearResultado)
    {
        var lista = new List<T>();
        using var conexion = ObtenerConexion();
        using var comando = new OracleCommand(sql, conexion);
        configurarParametros(comando);
        using var reader = comando.ExecuteReader();
        while (reader.Read())
            lista.Add(mapearResultado(reader));
        return lista;
    }

    /// <summary>
    /// Convierte un valor de Oracle a bool (0 = false, 1 = true).
    /// </summary>
    protected bool LeerBooleano(OracleDataReader reader, string columna)
    {
        int ordinal = reader.GetOrdinal(columna);
        if (reader.IsDBNull(ordinal)) return false;
        return reader.GetInt16(ordinal) == 1;
    }

    /// <summary>
    /// Convierte un valor de Oracle a string seguro (nunca null).
    /// </summary>
    protected string LeerString(OracleDataReader reader, string columna)
    {
        int ordinal = reader.GetOrdinal(columna);
        if (reader.IsDBNull(ordinal)) return string.Empty;
        return reader.GetString(ordinal);
    }

    /// <summary>
    /// Convierte un valor de Oracle a decimal? seguro.
    /// </summary>
    protected decimal? LeerDecimalNulo(OracleDataReader reader, string columna)
    {
        int ordinal = reader.GetOrdinal(columna);
        if (reader.IsDBNull(ordinal)) return null;
        return reader.GetDecimal(ordinal);
    }

    /// <summary>
    /// Convierte un valor de Oracle a int? seguro.
    /// </summary>
    protected int? LeerIntNulo(OracleDataReader reader, string columna)
    {
        int ordinal = reader.GetOrdinal(columna);
        if (reader.IsDBNull(ordinal)) return null;
        return reader.GetInt32(ordinal);
    }

    /// <summary>
    /// Convierte un valor de Oracle a DateTime? seguro.
    /// </summary>
    protected DateTime? LeerFechaNulo(OracleDataReader reader, string columna)
    {
        int ordinal = reader.GetOrdinal(columna);
        if (reader.IsDBNull(ordinal)) return null;
        return reader.GetDateTime(ordinal);
    }
}