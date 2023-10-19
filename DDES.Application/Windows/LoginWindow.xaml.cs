using DDES.Application.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace DDES.Application.Windows;

/// <summary>
/// Interaction logic for Login.xaml
/// </summary>
public sealed partial class LoginWindow : Window
{
    private readonly LoginViewModel _viewModel;

    public LoginWindow(LoginViewModel viewModel)
    {
        InitializeComponent();

        DataContext = _viewModel = viewModel;
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        _viewModel.User.Password = ((PasswordBox)sender).Password;
    }
}
