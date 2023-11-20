using DDES.Application.Services.Abstractions;
using Microsoft.AspNetCore.Components;

namespace DDES.Application.Shared;

public partial class MainLayout : LayoutComponentBase
{
    private Notification? _notification;

    [Inject]
    private ISubscriptionService SubscriptionService { get; set; } = default!;

    [Inject]
    private IAuthenticationService AuthenticationService { get; set; }
        = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        AuthenticationService.UserAuthenticated += _ =>
        {
            SubscriptionService.AddRoleBasedSubscriptions();
            
            SubscriptionService.MessageReceivedAsync += async (topic, message) =>
            {
                _notification = new Notification(topic, message);
                await InvokeAsync(StateHasChanged);
                await Task.Delay(5000);
                _notification = null;
            };
        };

        if (AuthenticationService.IsAuthenticated == false)
        {
            NavigationManager.NavigateTo("/login");
        }

        base.OnInitialized();
    }

    private record Notification(string Topic, string? Message);
}