using BLL;
using DAL;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Forms;
using System;

namespace GUI;

internal static class Program
{
    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    [STAThread]
    static void Main()
    {

        ApplicationConfiguration.Initialize();

        var servicios = new ServiceCollection();

        // ===== REGISTRAR CAPA DAL =====
        servicios.AgregarCapaDAL();

        // ===== REGISTRAR CAPA BLL =====
        servicios.AgregarCapaBLL();

        // ===== REGISTRAR FORMULARIOS =====
        // Registrar TODOS los formularios que usan DI
        servicios.AddTransient<LoginForm>();
        servicios.AddTransient<MainForm>();
        servicios.AddTransient<ProductosForm>();
        servicios.AddTransient<InventarioForm>();
        servicios.AddTransient<ClientesForm>();
        servicios.AddTransient<VentasForm>();
        servicios.AddTransient<UsuariosForm>();
        servicios.AddTransient<ReportesForm>();
        servicios.AddTransient<AlertasForm>();
        servicios.AddTransient<ConfiguracionForm>();
        servicios.AddTransient<DetallesProductoForm>();

        ServiceProvider = servicios.BuildServiceProvider();

        // Ejecutar la aplicación
        using (var scope = ServiceProvider.CreateScope())
        {
            var loginForm = scope.ServiceProvider.GetRequiredService<LoginForm>();
            Application.Run(loginForm);
        }
    }
}