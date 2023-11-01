using System.Text.Json;
using DDES.Common.Enums;
using DDES.Common.Helpers;
using DDES.Common.Models;
using DDES.Server.Services.Abstractions;
using NetMQ;
using NetMQ.Sockets;

namespace DDES.Server.Services;

internal class MessagingService
{
    private readonly ILogger<MessagingService> _logger;
    private readonly IUserService _userService;
    private readonly IClientService _clientService;

    public MessagingService(
        ILogger<MessagingService> logger,
        IUserService userService,
        IClientService clientService)
    {
        _logger = logger;
        _userService = userService;
        _clientService = clientService;
    }

    public void Listen(int port = 5555)
    {
        using ResponseSocket responder = new();
        responder.Bind($"tcp://*:{port}");

        _logger.LogInformation("Service listening on port: {port}.", port);

        while (true)
        {
            string message = responder.ReceiveFrameString();

            _logger.LogInformation("Received Frame: {message}", message);

            string response = ProcessMessage(message);

            _logger.LogInformation("Sending Frame: '{frame}'", response);

            responder.SendFrame(response);
        }
    }

    private string ProcessMessage(string message)
    {
        RequestMessage<string>? msg =
            EncryptionHelper.Decrypt<RequestMessage<string>>(message);

        if (msg is null)
        {
            return string.Empty;
        }

        _logger.LogInformation("{MessageType}, {Data}", msg.MessageType,
            msg.Data);

        if (msg.MessageType is MessageType.Authenticate)
        {
            ResponseMessage<User>? response = Authenticate(msg.Data.AsSpan());

            return EncryptionHelper.Encrypt(response);
        }

        return EncryptionHelper.Encrypt(ResponseMessage<string>.Empty);
    }

    private ResponseMessage<User>? Authenticate(ReadOnlySpan<char> message)
    {
        User? user = JsonSerializer.Deserialize<User>(message);

        if (user is null)
        {
            return null;
        }

        user = _userService.Authenticate(user.Username, user.Password);

        if (user is null)
        {
            return ResponseMessage<User>.Empty;
        }

        return new ResponseMessage<User>()
        {
            Successs = true,
            Content = user,
        };
    }
}