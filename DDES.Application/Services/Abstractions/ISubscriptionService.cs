namespace DDES.Application.Services.Abstractions;

public interface ISubscriptionService
{
    event Action<string, string?> MessageReceived;
    event Func<string, string?, Task> MessageReceivedAsync;

    Task SubscribeAsync(CancellationToken cancellationToken = default);

    void AddRoleBasedSubscriptions();

    void AddSubscription(string subscription);
}