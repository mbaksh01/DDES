using DDES.Application2.Services.Abstractions;
using DDES.Common.Enums;
using Microsoft.AspNetCore.Components;

namespace DDES.Application2.Pages;

public partial class Notifications : ComponentBase
{
    private readonly List<Notification> _generalNotifications = new();
    private readonly List<Notification> _personalNotifications = new();

    [Inject]
    private ISubscriptionService SubscriptionService { get; set; } = default!;

    protected override void OnInitialized()
    {
        SubscriptionService.MessageReceivedAsync += (topic, message) =>
        {
            switch (topic)
            {
                case Topics.PersonalNotification:
                    _personalNotifications.Add(new Notification(topic));
                    break;
                case Topics.GeneralNotification:
                default:
                    _generalNotifications.Add(new Notification(topic));
                    break;
            }

            return InvokeAsync(StateHasChanged);
        };

        base.OnInitialized();
    }

    record Notification(string Topic);
}