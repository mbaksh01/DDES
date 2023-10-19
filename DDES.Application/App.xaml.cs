using DDES.Application.Services;
using DDES.Application.Services.Abstractions;
using DDES.Application.ViewModels;
using DDES.Application.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace DDES.Application;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = Host
            .CreateDefaultBuilder()
            .ConfigureServices(RegisterServices)
            .Build();
    }

    private void RegisterServices(HostBuilderContext context, IServiceCollection services)
    {
        _ = services
            .AddSingleton<IMessagingService, MessagingService>()
            .AddSingleton<IViewControlService, ViewControlService>()
            .AddSingleton<IStateService, StateService>();

        _ = services
            .AddSingleton<LoginViewModel>();

        _ = services
            .AddSingleton<LoginWindow>()
            .AddSingleton<MainWindow>();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        AppHost!.Services.GetRequiredService<IViewControlService>().ShowWindow<LoginWindow>();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();

        base.OnExit(e);
    }
}

