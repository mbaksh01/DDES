using DDES.Server.Services.Abstractions;
using Microsoft.Extensions.Hosting;

namespace DDES.Server.Hosting;

internal sealed class InitializationService : IHostedService
{
    private readonly IMessagingService _messagingService;

    public InitializationService(IMessagingService messagingService)
    {
        _messagingService = messagingService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _messagingService.Listen();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _messagingService.Stop();
        return Task.CompletedTask;
    }
}