using DDES.Common.Models;
using DDES.Server.Services.Abstractions;

namespace DDES.Server.Services;

internal sealed class ClientService : IClientService
{
    private readonly List<Client> _clients = new();

    public void AddClient(Client client)
    {
        _clients.Add(client);
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
    }
}