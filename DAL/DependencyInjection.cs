using Microsoft.Extensions.DependencyInjection;
using DAL.Core;
using DAL.Interfaces;
using DAL.DAO;

namespace DAL;

public static class DependencyInjection
{
    public static IServiceCollection AgregarCapaDAL(this IServiceCollection servicios)
    {
        // Singleton para la fábrica de conexiones
        servicios.AddSingleton<IOracleConnectionFactory>(_ => OracleConnectionManager.Instancia);

        // DAOs con ciclo de vida Scoped
        servicios.AddScoped<IProductoDAO, ProductoDAO>();
        servicios.AddScoped<ILoteDAO, LoteDAO>();
        servicios.AddScoped<IVentaDAO, VentaDAO>();
        servicios.AddScoped<IDetalleVentaDAO, DetalleVentaDAO>();
        servicios.AddScoped<IAjusteInventarioDAO, AjusteInventarioDAO>();
        servicios.AddScoped<IUsuarioDAO, UsuarioDAO>();
        servicios.AddScoped<IClienteDAO, ClienteDAO>();
        servicios.AddScoped<IParametroSistemaDAO, ParametroSistemaDAO>();
        servicios.AddScoped<IHistorialParametroDAO, HistorialParametroDAO>();
        servicios.AddScoped<IHistorialDescuentoProductoDAO, HistorialDescuentoProductoDAO>();

        return servicios;
    }
}
