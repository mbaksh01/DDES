﻿using DDES.Application.Services.Abstractions;
using DDES.Common.Enums;
using DDES.Common.Models;

namespace DDES.Application.Services;

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
            .Send<ThreadMessageRequest, string>(
                MessageType.SendThreadMessage, new ThreadMessageRequest
                {
                    SupplierUsername = supplierName,
                    CustomerUsername = customerName,
                    Message = message,
                });
    }
}