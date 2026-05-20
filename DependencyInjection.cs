using Microsoft.Extensions.DependencyInjection;
using DAL.Core;
using DAL.Interfaces;
using DAL.Repositories;
using System;

namespace DAL;

/// <summary>
/// Configuración de inyección de dependencias para la capa DAL.
/// Principio aplicado: Dependency Inversion (DIP).
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra todos los servicios de la capa DAL en el contenedor de DI.
    /// </summary>
    public static IServiceCollection AgregarCapaDAL(this IServiceCollection servicios)
    {
        // Singleton para la fábrica de conexiones (una sola instancia en toda la aplicación)
        servicios.AddSingleton<IOracleConnectionFactory>(_ => OracleConnectionManager.Instancia);

        // Repositorios con ciclo de vida Scoped (uno por petición/operación)
        servicios.AddScoped<IProductoRepository, ProductoRepository>();
        servicios.AddScoped<ILoteRepository, LoteRepository>();
        servicios.AddScoped<IVentaRepository, VentaRepository>();
        servicios.AddScoped<IDetalleVentaRepository, DetalleVentaRepository>();
        servicios.AddScoped<IAjusteInventarioRepository, AjusteInventarioRepository>();
        servicios.AddScoped<IUsuarioRepository, UsuarioRepository>();
        servicios.AddScoped<IClienteRepository, ClienteRepository>();
        servicios.AddScoped<IParametroSistemaRepository, ParametroSistemaRepository>();
        servicios.AddScoped<IHistorialParametroRepository, HistorialParametroRepository>();
        servicios.AddScoped<IHistorialDescuentoProductoRepository, HistorialDescuentoProductoRepository>();

        return servicios;
    }
}