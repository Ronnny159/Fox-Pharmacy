using BLL.Interfaces;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BLL;

public static class DependencyInjection
{
    public static IServiceCollection AgregarCapaBLL(this IServiceCollection servicios)
    {
        // Servicios de negocio
        servicios.AddScoped<ILoginService, LoginService>();
        servicios.AddScoped<IUsuarioService, UsuarioService>();
        servicios.AddScoped<IProductoService, ProductoService>();
        servicios.AddScoped<ILoteService, LoteService>();
        servicios.AddScoped<IVentaService, VentaService>();
        servicios.AddScoped<IClienteService, ClienteService>();
        servicios.AddScoped<IParametroService, ParametroService>();
        servicios.AddScoped<IAlertaService, AlertaService>();
        servicios.AddScoped<IBIService, BIService>();
        servicios.AddScoped<IAuditoriaService, AuditoriaService>();

        return servicios;
    }
}