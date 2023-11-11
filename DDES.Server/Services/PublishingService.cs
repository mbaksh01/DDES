using DDES.Common.Helpers;
using NetMQ;
using NetMQ.Sockets;

namespace DDES.Server.Services;

public class PublishingService : IDisposable
{
    private readonly PublisherSocket _publisher;
    private const int PublishPort = 5554;

    public PublishingService()
    {
        _publisher = new PublisherSocket();
        _publisher.Bind($"tcp://*:{PublishPort}");
    }

    public void PublishMessage<TMessage>(string topic, TMessage message)
    {
        while (true)
        {
            string encryptedMessage = EncryptionHelper.Encrypt(message);

            PublishMessageEncrypted(topic, encryptedMessage);

            Thread.Sleep(1000);
        }
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