﻿using System.Text.Json;
using DDES.Application.Services.Abstractions;
using DDES.Common.Enums;
using DDES.Common.Helpers;
using DDES.Common.Models;
using NetMQ;
using NetMQ.Sockets;

namespace DDES.Application.Services;

public class MessagingService : IMessagingService
{
    private readonly IClientService _clientService;
    
    public int Port { get; }

    public MessagingService(IClientService clientService)
    {
        _clientService = clientService;
        Port = GetRandomPort();
    }

    public ResponseMessage<TResponse> Send<TModel, TResponse>(
        MessageType messageType,
        TModel data)
    {
        RequestMessage<string> message = new()
        {
            ClientId = _clientService.ClientId,
            MessageType = messageType,
            Content = JsonSerializer.Serialize(data),
        };

        using RequestSocket requestSocket = new();
        using ResponseSocket responseSocket = new();

        requestSocket.Connect("tcp://127.0.0.1:5555");

        string encryptedMessage = EncryptionHelper.Encrypt(message);

        responseSocket.Bind($"tcp://*:{Port}");
        //Send the request to the Server
        requestSocket.SendFrame(encryptedMessage);
        _ = requestSocket.ReceiveFrameString();

        //Receive response from the server and provide server with confirmation receipt
        string responseString = responseSocket.ReceiveFrameString();

        responseSocket.SendFrame("Message received by client");

        // responseSocket.Unbind($"tcp://*:{5556}");

        ResponseMessage<TResponse>? responseMessage =
            EncryptionHelper
                .Decrypt<ResponseMessage<TResponse>>(responseString);

        return responseMessage ??
               ResponseMessage<TResponse>.Empty;
    }

    private static int GetRandomPort()
    {
        return Random.Shared.Next(10_000, 60_000);
    }
}