using BLL.Interfaces;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BLL;

/// <summary>
/// Configuración de inyección de dependencias para la capa BLL.
/// Principio aplicado: Dependency Inversion (DIP).
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra todos los servicios de la capa BLL en el contenedor de DI.
    /// </summary>
    public static IServiceCollection AgregarCapaBLL(this IServiceCollection servicios)
    {
        servicios.AddScoped<IUsuarioService, UsuarioService>();
        servicios.AddScoped<IVentaService, VentaService>();
        servicios.AddScoped<IProductoService, ProductoService>();
        servicios.AddScoped<ILoteService, LoteService>();
        servicios.AddScoped<IParametroService, ParametroService>();
        servicios.AddScoped<IClienteService, ClienteService>();
        servicios.AddScoped<IBIService, BIService>();
        servicios.AddScoped<IAuditoriaService, AuditoriaService>();
        servicios.AddScoped<IAlertaService, AlertaService>();
        servicios.AddScoped<ILoginService, LoginService>();

        return servicios;
    }
}