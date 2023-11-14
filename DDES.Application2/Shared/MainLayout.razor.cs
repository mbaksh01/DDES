using DDES.Application2.Services.Abstractions;
using Microsoft.AspNetCore.Components;

namespace DDES.Application2.Shared;

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
        SubscriptionService.MessageReceivedAsync += async (topic, message) =>
        {
            _notification = new Notification(topic, message);
            await InvokeAsync(StateHasChanged);
            await Task.Delay(5000);
            _notification = null;
        };

        AuthenticationService.UserAuthenticated += _ =>
        {
            SubscriptionService.AddRoleBasedSubscriptions();
        };

        if (AuthenticationService.IsAuthenticated == false)
        {
            NavigationManager.NavigateTo("/login");
        }

        base.OnInitialized();
    }

    private record Notification(string Topic, string? Message);
}