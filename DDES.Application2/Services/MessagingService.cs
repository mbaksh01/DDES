using System.Text.Json;
using DDES.Application2.Services.Abstractions;
using DDES.Common.Enums;
using DDES.Common.Helpers;
using DDES.Common.Models;
using NetMQ;
using NetMQ.Sockets;

namespace DDES.Application2.Services;

public class MessagingService : IMessagingService
{
    public ResponseMessage<TResponse> Send<TModel, TResponse>(Guid clientId,
        MessageType messageType, TModel data)
    {
        RequestMessage<string> message = new()
        {
            MessageType = messageType,
            Data = JsonSerializer.Serialize(data),
        };

        string encryptedMessage = EncryptionHelper.Encrypt(message);

        using RequestSocket requestSocket = GetRequestSocket();

        requestSocket.SendFrame(encryptedMessage);

        Thread.Sleep(1000);

        string responseString = requestSocket.ReceiveFrameString();

        ResponseMessage<TResponse>? responseMessage =
            EncryptionHelper
                .Decrypt<ResponseMessage<TResponse>>(responseString);

        return responseMessage ?? ResponseMessage<TResponse>.Empty;
    }

    private static RequestSocket GetRequestSocket()
    {
        RequestSocket requester = new();

        requester.Connect("tcp://localhost:5555");

        return requester;
    }
}