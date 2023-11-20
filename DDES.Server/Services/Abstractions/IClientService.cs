using DDES.Common.Models;

namespace DDES.Server.Services.Abstractions;

internal interface IClientService
{
    void AddClient(Client client);

    string? GetUsername(Guid clientId);

    void AppendUsername(Guid clientId, string username);
    
    int GetPort(Guid clientId);
}