using DDES.Application.Services.Abstractions;
using DDES.Common.Enums;
using DDES.Common.Models;
using Microsoft.AspNetCore.Components;
using Thread = System.Threading.Thread;

namespace DDES.Application;

public partial class Main : ComponentBase
{
    [Inject] private IClientService ClientService { get; set; } = default!;

    [Inject]
    private IMessagingService MessagingService { get; set; } = default!;

    [Inject]
    private ISubscriptionService SubscriptionService { get; set; } = default!;

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

        Thread thread = new(() => SubscriptionService.SubscribeAsync());
        thread.Start();

        base.OnInitialized();
    }
}