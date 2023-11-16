using DDES.Application.Services.Abstractions;

namespace DDES.Application.Services;

public class ClientService : IClientService
{
    public Guid ClientId { get; set; }
}