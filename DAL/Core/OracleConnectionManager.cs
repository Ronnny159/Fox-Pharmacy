using Oracle.ManagedDataAccess.Client;

namespace DAL.Core;

/// <summary>
/// Implementación Singleton thread-safe de la fábrica de conexiones Oracle.
/// Garantiza una única instancia de configuración durante toda la vida de la aplicación.
/// Patrón aplicado: Singleton (con Lazy<T> para thread-safety).
/// </summary>
public class OracleConnectionManager : IOracleConnectionFactory
{
    private readonly string _cadenaConexion;
    private static readonly Lazy<OracleConnectionManager> _instancia =
        new(() => new OracleConnectionManager());

    /// <summary>
    /// Constructor privado. Configura la cadena de conexión.
    /// Modificar aquí para apuntar a tu instancia de Oracle.
    /// </summary>
    private OracleConnectionManager()
{
    string host = "localhost";
    string puerto = "1521";
    string servicio = "XE";
    string usuario = "USUARIO_LUIS";
    string password = "POWER140507";
    
    _cadenaConexion = $"User Id={usuario};Password={password};Data Source={host}:{puerto}/{servicio};";
    
    Console.WriteLine($"Usando conexión: {_cadenaConexion}");
}

    /// <summary>
    /// Instancia única del administrador de conexiones.
    /// </summary>
    public static OracleConnectionManager Instancia => _instancia.Value;

    /// <summary>
    /// Crea una nueva conexión abierta a Oracle.
    /// Cada llamada devuelve una conexión nueva para evitar estados compartidos.
    /// </summary>
    public OracleConnection CrearConexion()
    {
        var conexion = new OracleConnection(_cadenaConexion);
        conexion.Open();
        return conexion;
    }
}