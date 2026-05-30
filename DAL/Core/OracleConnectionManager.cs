using Oracle.ManagedDataAccess.Client;
using System.Configuration;

namespace DAL.Core;

/// <summary>
/// Implementación Singleton thread-safe de la fábrica de conexiones Oracle.
/// </summary>
public class OracleConnectionManager : IOracleConnectionFactory
{
    private readonly string _cadenaConexion;
    private static readonly Lazy<OracleConnectionManager> _instancia =
        new(() => new OracleConnectionManager());

    /// <summary>
    /// Constructor privado. Lee la cadena de conexión desde App.config.
    /// </summary>
    private OracleConnectionManager()
    {
        _cadenaConexion = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;

        if (string.IsNullOrEmpty(_cadenaConexion))
        {
            throw new Exception("No se encontró la cadena de conexión 'OracleConnection' en App.config");
        }
    }

    /// <summary>
    /// Instancia única del administrador de conexiones.
    /// </summary>
    public static OracleConnectionManager Instancia => _instancia.Value;

    /// <summary>
    /// Crea una nueva conexión abierta a Oracle.
    /// </summary>
    public OracleConnection CrearConexion()
    {
        var conexion = new OracleConnection(_cadenaConexion);
        conexion.Open();
        return conexion;
    }

    /// <summary>
    /// Método de prueba para verificar la conexión
    /// </summary>
    public bool ProbarConexion()
    {
        try
        {
            using (var conexion = new OracleConnection(_cadenaConexion))
            {
                conexion.Open();
                Console.WriteLine(" Conexión exitosa a Oracle");
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(" Error de conexión: " + ex.Message);
            return false;
        }
    }
}