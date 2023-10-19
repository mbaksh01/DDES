using System.Windows;

namespace DDES.Application.Services.Abstractions;

public interface IViewControlService
{
    Window CurrentWindow { get; }

    void ShowWindow<TWindow>() where TWindow : Window;
}