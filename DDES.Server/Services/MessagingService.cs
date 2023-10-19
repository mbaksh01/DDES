using DDES.Data.Enums;
using DDES.Data.Models;
using DDES.Server.Services.Abstractions;
using NetMQ;
using NetMQ.Sockets;
using System.Text.Json;

namespace DDES.Server.Services;

internal class MessagingService : IDisposable
{
    private readonly ILogger<MessagingService> _logger;
    private readonly IUserService _userService;
    private readonly IClientService _clientService;
    private readonly IEncryptionService _encryptionService;

    public MessagingService(
        ILogger<MessagingService> logger,
        IUserService userService,
        IClientService clientService,
        IEncryptionService encryptionService)
    {
        _logger = logger;
        _userService = userService;
        _clientService = clientService;
        _encryptionService = encryptionService;
    }

    public void Listen(int port = 5555)
    {
        using ResponseSocket responder = new();
        responder.Bind($"tcp://*:{port}");

        _logger.LogInformation($"Service listening on port: {port}.");

        while (true)
        {
            string message = responder.ReceiveFrameString();

            _logger.LogInformation("Received Frame: {message}", message);

            byte[] response = ProcessMessage(message);

            _logger.LogInformation("Sending Frame: '{frame}'", string.Join(" ", response.Select(b => $"0x{b:X2}")));

            responder.SendFrame(response);
        }
    }

    private byte[] ProcessMessage(string message)
    {
        Message? msg = _encryptionService.Decrypt<Message>(message);

        if (msg is null)
        {
            return Array.Empty<byte>();
        }

        Message? response = msg.MessageType switch
        {
            MessageType.ClientConnected => ClientConnected(msg.Data.AsSpan()),
            MessageType.Authenticate => Authenticate(msg.Data.AsSpan()),
            _ or MessageType.Unknown => Message.CreateResponseMessage(null),
        };

        return response?.Data is null
            ? Array.Empty<byte>()
            : _encryptionService.Encrypt(response, msg.ClientId);
    }

    private Message? Authenticate(ReadOnlySpan<char> message)
    {
        User? user = JsonSerializer.Deserialize<User>(message);

        if (user is null)
        {
            return null;
        }

        user = _userService.Authenticate(user.Username, user.Password);

        return Message.CreateResponseMessage(user is null ? null : JsonSerializer.Serialize(user));
    }

    private Message? ClientConnected(ReadOnlySpan<char> data)
    {
        byte[]? clientPublicKey = JsonSerializer.Deserialize<byte[]>(data);

        if (clientPublicKey is null)
        {
            return null;
        }

        Client client = new(Guid.NewGuid(), clientPublicKey, string.Empty);

        _clientService.AddClient(client);

        return Message.CreateResponseMessage("");
    }

    public void Dispose()
    {
        _encryptionService.Dispose();
    }
}
