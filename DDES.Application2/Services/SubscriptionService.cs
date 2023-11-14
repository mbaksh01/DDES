using DDES.Application2.Services.Abstractions;
using DDES.Common.Enums;
using DDES.Common.Helpers;
using DDES.Common.Models;
using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;

namespace DDES.Application2.Services;

public sealed class SubscriptionService : ISubscriptionService, IDisposable
{
    private readonly ILogger<SubscriptionService> _logger;
    private readonly IMessagingService _messagingService;
    private readonly SubscriberSocket _subscriber;
    private readonly IAuthenticationService _authenticationService;

    public event Action<string, string?>? MessageReceived;

    public event Func<string, string?, Task>? MessageReceivedAsync;

    public SubscriptionService(
        ILogger<SubscriptionService> logger,
        IAuthenticationService authenticationService,
        IMessagingService messagingService)
    {
        _logger = logger;
        _authenticationService = authenticationService;
        _messagingService = messagingService;

        SubscriberSocket subscriber = new();
        subscriber.Connect("tcp://127.0.0.1:5554");

        _subscriber = subscriber;
    }

    public async Task SubscribeAsync(
        CancellationToken cancellationToken = default)
    {
        _subscriber.Subscribe(Topics.PersonalNotification);
        _subscriber.Subscribe(Topics.GeneralNotification);
        _subscriber.Subscribe("test");

        if (_authenticationService.User?.Roles.Contains("customer") ?? false)
        {
            _subscriber.Subscribe(Topics.CustomerNotification);
        }

        while (cancellationToken.IsCancellationRequested == false)
        {
            string topic = _subscriber.ReceiveFrameString();
            string message = _subscriber.ReceiveFrameString();

            string? decryptedMessage =
                EncryptionHelper.Decrypt<string>(message);

            MessageReceived?.Invoke(topic, decryptedMessage);

            if (MessageReceivedAsync is not null)
            {
                await MessageReceivedAsync.Invoke(topic, decryptedMessage);
            }
        }
    }

    public void AddRoleBasedSubscriptions()
    {
        if (_authenticationService.User?.Roles.Contains("customer") ?? false)
        {
            _subscriber.Subscribe(Topics.CustomerNotification);
        }
    }

    public void AddSubscription(string subscription)
    {
        ResponseMessage<int> responseMessage = _messagingService
            .Send<string, int>(MessageType.UpdateSubscription, subscription);

        if (responseMessage.Successs)
        {
            _authenticationService.User!.Subscriptions.Add("subscription");
        }
    }

    public void Dispose()
    {
        _subscriber.Dispose();
    }
}