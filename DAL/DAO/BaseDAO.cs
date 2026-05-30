using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using System.Data;

namespace DAL.DAO;

/// <summary>
/// Clase base abstracta para todos los DAO.
/// Proporciona métodos comunes para llamar procedimientos almacenados
/// y mapear cursores de salida. CERO SQL en C#.
/// Principio aplicado: DRY, Template Method (GRASP).
/// </summary>
public abstract class BaseDAO
{
    protected readonly IOracleConnectionFactory _conexionFactory;

    protected BaseDAO(IOracleConnectionFactory conexionFactory)
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
    /// Ejecuta un procedimiento almacenado que retorna un cursor.
    /// CORREGIDO: Agrega automáticamente el parámetro REF CURSOR de salida.
    /// </summary>
    protected void EjecutarCursor(string nombreProcedimiento, Action<OracleCommand> configurarParametros, Action<OracleDataReader> procesarCursor)
    {
        using var conexion = ObtenerConexion();
        using var comando = new OracleCommand(nombreProcedimiento, conexion);
        comando.CommandType = CommandType.StoredProcedure;

        // 1. Configurar los parámetros de entrada (p_nombre, p_hash, etc.)
        configurarParametros(comando);

        // 2. Agregar el parámetro de salida REF CURSOR (importante: mismo nombre que en Oracle)
        // Normalmente se llama "p_cursor", "p_refcursor" o "cursor"
        // Si tu procedimiento usa otro nombre, cámbialo aquí
        comando.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

        // 3. Ejecutar y obtener el reader (Oracle automáticamente asigna el cursor al parámetro de salida)
        using var reader = comando.ExecuteReader();

        // 4. Procesar los resultados
        procesarCursor(reader);
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

    /// <summary>
    /// Convierte un valor de Oracle a char seguro.
    /// </summary>
    protected char LeerChar(OracleDataReader reader, string columna)
    {
        int ordinal = reader.GetOrdinal(columna);
        if (reader.IsDBNull(ordinal)) return ' ';
        string valor = reader.GetString(ordinal);
        return string.IsNullOrEmpty(valor) ? ' ' : valor[0];
    }
}