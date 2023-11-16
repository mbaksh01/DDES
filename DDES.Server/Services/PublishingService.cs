using DDES.Common.Helpers;
using DDES.Server.Services.Abstractions;
using NetMQ;
using NetMQ.Sockets;

namespace DDES.Server.Services;

public class PublishingService : IPublishingService
{
    private readonly PublisherSocket _publisher;
    private const int PublishPort = 5554;
    private readonly ILogger<PublishingService> _logger;

    public PublishingService(ILogger<PublishingService> logger)
    {
        _logger = logger;
        _publisher = new PublisherSocket();
        _publisher.Bind($"tcp://*:{PublishPort}");
    }

    public void PublishMessage<TMessage>(string topic, TMessage message)
    {
        string encryptedMessage = EncryptionHelper.Encrypt(message);

        PublishMessageEncrypted(topic, encryptedMessage);
        _logger.LogInformation("Message published, Topic: {topic}.", topic);
    }

    private void PublishMessageEncrypted(string topic, string encryptedMessage)
    {
        _publisher
            .SendMoreFrame(topic)
            .SendFrame(encryptedMessage);
    }

    public void Dispose()
    {
        _publisher.Dispose();
        GC.SuppressFinalize(this);
    }
}