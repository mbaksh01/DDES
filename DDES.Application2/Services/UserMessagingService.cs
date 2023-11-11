using DDES.Application2.Services.Abstractions;
using DDES.Common.Enums;
using DDES.Common.Models;

namespace DDES.Application2.Services;

public class UserMessagingService : IUserMessagingService
{
    private readonly IMessagingService _messagingService;
    private readonly IClientService _clientService;

    public UserMessagingService(
        IMessagingService messagingService,
        IClientService clientService)
    {
        _messagingService = messagingService;
        _clientService = clientService;
    }

    public void SendMessage(
        string supplierName,
        string customerName,
        ThreadMessage message)
    {
        _messagingService
            .Send<ThreadMessageRequest, ResponseMessage<string>>(
                MessageType.SendThreadMessage, new ThreadMessageRequest
                {
                    SupplierUsername = supplierName,
                    CustomerUsername = customerName,
                    Message = message,
                });
    }
}