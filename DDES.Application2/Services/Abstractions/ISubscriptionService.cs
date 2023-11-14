namespace DDES.Application2.Services.Abstractions;

public interface ISubscriptionService
{
    event Action<string, string?> MessageReceived;
    event Func<string, string?, Task> MessageReceivedAsync;

    Task SubscribeAsync(CancellationToken cancellationToken = default);

    void AddRoleBasedSubscriptions();
}