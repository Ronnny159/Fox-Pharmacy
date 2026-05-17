using Oracle.ManagedDataAccess.Client;

namespace DAL.Core;

/// <summary>
/// Abstracción para la creación de conexiones a Oracle.
/// Permite desacoplar la fuente de datos y facilita pruebas unitarias.
/// Principio aplicado: Dependency Inversion (DIP).
/// </summary>
public interface IOracleConnectionFactory
{
    /// <summary>
    /// Crea y devuelve una nueva conexión abierta a Oracle.
    /// </summary>
    OracleConnection CrearConexion();
}