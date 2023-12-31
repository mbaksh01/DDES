﻿using DDES.Application.Services.Abstractions;
using DDES.Common.Enums;
using Microsoft.AspNetCore.Components;

namespace DDES.Application.Pages;

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

    private void ClearAllPersonalNotifications()
    {
        _personalNotifications.Clear();
    }

    private void ClearAllGeneralNotifications()
    {
        _generalNotifications.Clear();
    }

    record Notification(string Topic);
}