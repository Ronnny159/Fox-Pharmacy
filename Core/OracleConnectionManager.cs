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
        // CONFIGURAR: Cambia estos valores por los de tu entorno Oracle
        _cadenaConexion = "User Id=pharmauser;Password=pharmapass;Data Source=localhost:1521/XE;";
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