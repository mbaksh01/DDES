﻿using System.Text.Json;
using DDES.Application.Services.Abstractions;
using DDES.Common.Enums;
using DDES.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Thread = DDES.Common.Models.Thread;

namespace DDES.Application.Pages;

public partial class Messages : ComponentBase
{
    private List<Thread> _threads = new();

    private Thread? _currentThread;

    private string _currentRole = string.Empty;

    private string _currentUsername = string.Empty;

    private string _message = string.Empty;

    [Inject]
    private IMessagingService MessagingService { get; set; } = default!;

    [Inject]
    private IAuthenticationService AuthenticationService { get; set; }
        = default!;

    [Inject]
    private IUserMessagingService UserMessagingService { get; set; } = default!;

    [Inject]
    private ISubscriptionService SubscriptionService { get; set; } = default!;

    protected override void OnInitialized()
    {
        ResponseMessage<Threads> response =
            MessagingService.Send<int, Threads>(MessageType.GetThreads, 0);

        if (response.Successs == false)
        {
            base.OnInitialized();
            return;
        }

        _threads = response.Content!.ThreadList;
        _currentThread = _threads.FirstOrDefault();

        _currentRole =
            AuthenticationService.User?.Roles.First() ?? string.Empty;

        _currentUsername = AuthenticationService.User?.Username ?? string.Empty;

        SubscriptionService.MessageReceivedAsync += (topic, message) =>
        {
            if (topic != Topics.NewDirectMessage)
            {
                return Task.CompletedTask;
            }

            ThreadMessage threadMessage =
                JsonSerializer.Deserialize<ThreadMessage>(message!)!;

            _currentThread?.Messages.Add(threadMessage);

            return InvokeAsync(StateHasChanged);
        };
    }

    private void SendOnEnter(KeyboardEventArgs args)
    {
        if (args.Key == "Enter")
        {
            SendMessage();
        }
    }

    private void SendMessage()
    {
        if (_currentThread is null || string.IsNullOrWhiteSpace(_message))
        {
            return;
        }

        ThreadMessage threadMessage = new()
        {
            From = _currentRole,
            To = _currentRole.Equals("customer",
                StringComparison.OrdinalIgnoreCase)
                ? "supplier"
                : "customer",
            MessageText = _message,
            DateTimeSent = DateTime.Now,
        };

        _currentThread.Messages.Add(threadMessage);

        UserMessagingService.SendMessage(
            "supplier",
            "customer",
            threadMessage);

        _message = string.Empty;
    }
}