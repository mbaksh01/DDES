using DDES.Application.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DDES.Application.Services;

internal sealed class ViewControlService : IViewControlService
{
    private readonly IServiceProvider _serviceProvider;

    public Window CurrentWindow { get; private set; }

    public ViewControlService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void ShowWindow<TWindow>() where TWindow : Window
    {
        Window window = _serviceProvider.GetRequiredService<TWindow>();

        window.Show();

        CurrentWindow?.Close();

        CurrentWindow = window;
    }
}
