using DDES.Common.Models;

namespace DDES.Server.Services.Abstractions;

internal interface IClientService
{
    void AddClient(Client client);

    byte[] GetPublicKey(Guid id);
}