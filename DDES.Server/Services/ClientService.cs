using DDES.Common.Models;
using DDES.Server.Services.Abstractions;

namespace DDES.Server.Services;

internal sealed class ClientService : IClientService
{
    private readonly ILogger<ClientService> _logger;
    private readonly List<Client> _clients = new();

    public ClientService(ILogger<ClientService> logger)
    {
        _logger = logger;
    }

    public void AddClient(Client client)
    {
        _clients.Add(client);

        _logger.LogInformation(
            "Added client. ClientId {ClientId}, Username: {Username}, Port: {Port}.",
            client.Id,
            client.Username,
            client.Port);
    }

    public string? GetUsername(Guid clientId)
    {
        return _clients.Find(c => c.Id == clientId)?.Username;
    }

    public void AppendUsername(Guid clientId, string username)
    {
        var client = _clients.Find(c => c.Id == clientId);

        if (client is null)
        {
            return;
        }

        _clients.Remove(client);
        _clients.Add(client with { Username = username });

        _logger.LogInformation(
            "Successfully updated client with new username. ClientId: {ClientId}, Username: {Username}",
            clientId,
            username);
    }

    public int GetPort(Guid clientId)
    {
        Client? client = _clients.Find(c => c.Id == clientId);

        if (client is null)
        {
            throw new Exception(
                $"Could not find the client with id: {clientId}");
        }

        return client.Port;
    }
}