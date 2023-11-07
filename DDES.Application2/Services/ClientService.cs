using DDES.Application2.Services.Abstractions;

namespace DDES.Application2.Services;

public class ClientService : IClientService
{
    public Guid ClientId { get; set; }
}