﻿using DDES.Application2.Services.Abstractions;
using DDES.Common.Enums;
using DDES.Common.Models;
using Microsoft.AspNetCore.Components;

namespace DDES.Application2;

public partial class Main : ComponentBase
{
    [Inject] private IClientService ClientService { get; set; } = default!;

    [Inject]
    private IMessagingService MessagingService { get; set; } = default!;

    protected override void OnInitialized()
    {
        ResponseMessage<ClientConnectedResponse> response = MessagingService
            .Send<ClientConnectedRequest, ClientConnectedResponse>(
                MessageType.ClientConnected, new ClientConnectedRequest
                {
                    Username = "",
                    Port = 5556,
                });

        if (response.Successs)
        {
            ClientService.ClientId = response.Content!.ClientId;
        }

        base.OnInitialized();
    }
}