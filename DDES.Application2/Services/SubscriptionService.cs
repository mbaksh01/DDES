using DDES.Application2.Services.Abstractions;
using DDES.Common.Enums;
using DDES.Common.Helpers;
using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;

namespace DDES.Application2.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ILogger<SubscriptionService> _logger;

    public event Action<string, string?>? MessageReceived;

    public event Func<string, string?, Task>? MessageReceivedAsync;

    public SubscriptionService(ILogger<SubscriptionService> logger)
    {
        _logger = logger;
    }

    public async Task SubscribeAsync(
        CancellationToken cancellationToken = default)
    {
        using SubscriberSocket subscriber = new();
        subscriber.Connect("tcp://127.0.0.1:5554");
        subscriber.Subscribe(Topics.PersonalNotification);
        subscriber.Subscribe(Topics.GeneralNotification);
        subscriber.Subscribe("Test");

        while (cancellationToken.IsCancellationRequested == false)
        {
            string topic = subscriber.ReceiveFrameString();
            string message = subscriber.ReceiveFrameString();

            string? decryptedMessage =
                EncryptionHelper.Decrypt<string>(message);

            MessageReceived?.Invoke(topic, decryptedMessage);

            if (MessageReceivedAsync is not null)
            {
                await MessageReceivedAsync.Invoke(topic, decryptedMessage);
            }
        }
    }
}