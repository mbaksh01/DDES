using DDES.Common.Models;
using DDES.Server.Services.Abstractions;

namespace DDES.Server.Services;

internal sealed class ClientService : IClientService
{
    private readonly List<Client> _clients = new();

    public byte[] GetPublicKey(Guid id)
    {
        return _clients.Find(x => x.Id == id)?.PublicKey ?? Array.Empty<byte>();
    }

    public void AddClient(Client client)
    {
        _clients.Add(client);
    }
}