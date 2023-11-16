namespace DDES.Server.Services.Abstractions;

public interface IPublishingService : IDisposable
{
    void PublishMessage<TMessage>(string topic, TMessage message);
}