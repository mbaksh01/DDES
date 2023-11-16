using DDES.Application.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DDES.Application.Pages;

public sealed partial class Login : ComponentBase
{
    private string _username = string.Empty;

    private string _password = string.Empty;

    [Inject]
    private IAuthenticationService AuthenticationService { get; set; } =
        default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private void LoginClicked()
    {
        if (AuthenticationService.Authenticate(_username, _password, out _))
        {
            NavigationManager.NavigateTo("");
        }
    }

    private void LoginOnEnter(KeyboardEventArgs args)
    {
        if (args.Key == "Enter")
        {
            LoginClicked();
        }
    }
}