using CommunityToolkit.Mvvm.Input;
using DDES.Application.Services.Abstractions;
using DDES.Application.Windows;
using DDES.Common.Models;

namespace DDES.Application.ViewModels;

public class LoginViewModel
{
    private readonly IMessagingService _messagingService;
    private readonly IStateService _stateService;
    private readonly IViewControlService _viewControlService;

    public User User { get; set; } = new User()
    {
        Username = string.Empty,
        Password = string.Empty,
        Roles = new List<string>(),
    };

    public RelayCommand LoginCommand { get; set; }

    public LoginViewModel(
        IMessagingService messagingService,
        IStateService stateService,
        IViewControlService viewControlService)
    {
        _messagingService = messagingService;
        _stateService = stateService;
        _viewControlService = viewControlService;

        LoginCommand = new RelayCommand(Login);
    }

    private void Login()
    {
        User? responseUser = _messagingService.Authenticate(User);

        if (responseUser is null)
        {
            // TODO: Notify the user that the credentials were not correct
            return;
        }

        _stateService.CurrentUser = responseUser;

        _viewControlService.ShowWindow<MainWindow>();
    }
}