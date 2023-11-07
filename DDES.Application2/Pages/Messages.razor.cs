using DDES.Application2.Services.Abstractions;
using DDES.Common.Enums;
using DDES.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Thread = DDES.Common.Models.Thread;

namespace DDES.Application2.Pages;

public partial class Messages : ComponentBase
{
    private List<Thread> _threads = new();

    private Thread? _currentThread;

    private string _currentRole = string.Empty;

    private string _message = string.Empty;

    [Inject] private IMessagingService MessagingService { get; set; }

    [Inject] private IAuthenticationService AuthenticationService { get; set; }

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

        _currentThread.Messages.Add(new ThreadMessage
        {
            From = _currentRole,
            To = _currentRole.Equals("customer",
                StringComparison.OrdinalIgnoreCase)
                ? "supplier"
                : "customer",
            MessageText = _message,
            DateTimeSent = DateTime.Now,
        });

        _message = string.Empty;
    }
}