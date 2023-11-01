using System.Buffers;
using System.Text;
using System.Text.Json;
using DDES.Application.Services.Abstractions;
using DDES.Common.Enums;
using DDES.Common.Models;
using NetMQ;
using NetMQ.Sockets;

namespace DDES.Application.Services;

internal sealed class MessagingService : IMessagingService
{
    public User? Authenticate(User user)
    {
        string jsonUser = JsonSerializer.Serialize(user);

        string response = SendMessage(jsonUser);

        return string.IsNullOrEmpty(response)
            ? null
            : JsonSerializer.Deserialize<User?>(response);
    }

    public string SendMessage(string data)
    {
        using RequestSocket requester = GetRequestSocket();

        byte[] buffer = ArrayPool<byte>.Shared.Rent(data.Length + 3);

        int writtenBytes =
            Encoding.UTF8.GetBytes(
                $"{(int)MessageType.Authenticate}-{data}".AsSpan(), buffer);

        requester.SendFrame(buffer[..writtenBytes]);

        ArrayPool<byte>.Shared.Return(buffer);

        return requester.ReceiveFrameString();
    }

    private static RequestSocket GetRequestSocket()
    {
        RequestSocket requester = new();

        requester.Connect("tcp://localhost:5555");

        return requester;
    }
}