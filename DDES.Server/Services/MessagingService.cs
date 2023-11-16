using System.Text.Json;
using DDES.Common.Enums;
using DDES.Common.Helpers;
using DDES.Common.Models;
using DDES.Server.Services.Abstractions;
using NetMQ;
using NetMQ.Sockets;
using Thread = DDES.Common.Models.Thread;

namespace DDES.Server.Services;

internal class MessagingService : IMessagingService
{
    private readonly ILogger<MessagingService> _logger;
    private readonly IUserService _userService;
    private readonly IUserMessagingService _userMessaging;
    private readonly IClientService _clientService;
    private readonly IProductService _productService;
    private readonly IPublishingService _publishingService;

    public MessagingService(
        ILogger<MessagingService> logger,
        IUserService userService,
        IClientService clientService,
        IUserMessagingService userMessaging,
        IProductService productService,
        IPublishingService publishingService)
    {
        _logger = logger;
        _userService = userService;
        _clientService = clientService;
        _userMessaging = userMessaging;
        _productService = productService;
        _publishingService = publishingService;
    }

    public void Listen(int port = 5555)
    {
        using ResponseSocket receiver = new();
        receiver.Bind($"tcp://*:{port}");

        while (true)
        {
            _logger.LogInformation("Service listening on port: {port}.",
                port);
            string message = receiver.ReceiveFrameString();
            receiver.SendFrameEmpty();

            _logger.LogInformation("Received Frame: {message}", message);

            string response = ProcessMessage(message);

            _logger.LogInformation("Sending Frame: '{frame}'", response);

            using RequestSocket responder = new();
            responder.Connect($"tcp://127.0.0.1:{5556}");
            responder.SendFrame(response);
            responder.ReceiveFrameString();
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

        _logger.LogInformation(
            "Processing message. Message Type: {messageType}, Content: {content}.",
            msg.MessageType,
            msg.Content);

        return msg.MessageType switch
        {
            MessageType.Authenticate => EncryptionHelper.Encrypt(
                Authenticate(msg.ClientId, msg.Content)),
            MessageType.GetThreads => EncryptionHelper.Encrypt(
                GetThreads(msg.ClientId)),
            MessageType.ClientConnected => EncryptionHelper.Encrypt(
                ClientConnected(msg.Content)),
            MessageType.SendThreadMessage => EncryptionHelper.Encrypt(
                ReceiveThreadMessage(msg.Content)),
            MessageType.GetProducts => EncryptionHelper.Encrypt(GetProducts()),
            MessageType.AddProduct => EncryptionHelper.Encrypt(
                AddProduct(msg.Content)),
            MessageType.UpdateProduct => EncryptionHelper.Encrypt(
                UpdateProduct(msg.Content)),
            MessageType.DeleteProduct => EncryptionHelper.Encrypt(
                DeleteProduct(msg.Content)),
            MessageType.BroadcastMessage => EncryptionHelper.Encrypt(
                BroadcastMessage(msg.Content)),
            MessageType.UpdateSubscription => EncryptionHelper.Encrypt(
                AddSubscription(msg.ClientId, msg.Content)),
            _ => EncryptionHelper.Encrypt(ResponseMessage<string>.Empty),
        };
    }

    private ResponseMessage<User> Authenticate(
        Guid clientId,
        ReadOnlySpan<char> message)
    {
        User? user = JsonSerializer.Deserialize<User>(message);

        if (user is null)
        {
            return ResponseMessage<User>.Empty;
        }

        user = _userService.Authenticate(user.Username, user.Password);

        if (user is null)
        {
            return ResponseMessage<User>.Empty;
        }

        _clientService.AppendUsername(clientId, user.Username);

        _logger.LogInformation("Successfully authenticated user.");

        return new ResponseMessage<User>
        {
            Successs = true,
            Content = user,
        };
    }

    private ResponseMessage<Threads> GetThreads(Guid clientId)
    {
        string? username = _clientService.GetUsername(clientId);

        if (username is null)
        {
            return ResponseMessage<Threads>.Empty;
        }

        IEnumerable<Thread> threads = _userMessaging.GetThreads(username);

        _logger.LogInformation("Successfully got threads.");

        return new ResponseMessage<Threads>
        {
            Successs = true,
            Content = new Threads
            {
                ThreadList = threads.ToList(),
            },
        };
    }

    private ResponseMessage<string> ReceiveThreadMessage(
        ReadOnlySpan<char> threadMessageJson)
    {
        ThreadMessageRequest? request =
            JsonSerializer.Deserialize<ThreadMessageRequest>(threadMessageJson);

        if (request is null)
        {
            return ResponseMessage<string>.Empty;
        }

        _userMessaging.AddThreadMessage(
            request.SupplierUsername,
            request.CustomerUsername,
            request.Message);

        return new ResponseMessage<string>()
        {
            Successs = true,
        };
    }

    private ResponseMessage<ClientConnectedResponse> ClientConnected(
        ReadOnlySpan<char> clientConnectedJson)
    {
        ClientConnectedRequest? clientConnected =
            JsonSerializer.Deserialize<ClientConnectedRequest>(
                clientConnectedJson);

        if (clientConnected is null)
        {
            return ResponseMessage<ClientConnectedResponse>.Empty;
        }

        Client client = new(
            Guid.NewGuid(),
            clientConnected.Username,
            clientConnected.Port);

        _clientService.AddClient(client);

        _logger.LogInformation("Successfully registered client.");

        return new ResponseMessage<ClientConnectedResponse>
        {
            Successs = true,
            Content = new ClientConnectedResponse
            {
                ClientId = client.Id,
            }
        };
    }

    private ResponseMessage<List<Product>> GetProducts()
    {
        return new ResponseMessage<List<Product>>()
        {
            Successs = true,
            Content = _productService.GetAllProducts(),
        };
    }

    private ResponseMessage<Product> AddProduct(ReadOnlySpan<char> productJson)
    {
        Product? product = JsonSerializer.Deserialize<Product>(productJson);

        if (product is null)
        {
            return ResponseMessage<Product>.Empty;
        }

        _productService.AddProduct(product);

        return new ResponseMessage<Product>
        {
            Successs = true,
            Content = product,
        };
    }

    private ResponseMessage<Product> UpdateProduct(
        ReadOnlySpan<char> productJson)
    {
        Product? product = JsonSerializer.Deserialize<Product>(productJson);

        if (product is null)
        {
            return ResponseMessage<Product>.Empty;
        }

        _productService.UpdateProduct(product);

        return new ResponseMessage<Product>
        {
            Successs = true,
            Content = product,
        };
    }

    private ResponseMessage<Product> DeleteProduct(
        ReadOnlySpan<char> productJson)
    {
        Product? product = JsonSerializer.Deserialize<Product>(productJson);

        if (product is null)
        {
            return ResponseMessage<Product>.Empty;
        }

        _productService.DeleteProduct(product);

        return new ResponseMessage<Product>
        {
            Successs = true,
            Content = product,
        };
    }

    private ResponseMessage<int> BroadcastMessage(string? message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return ResponseMessage<int>.Empty;
        }

        _publishingService.PublishMessage(Topics.CustomerNotification, message);

        return new ResponseMessage<int>
        {
            Successs = true,
            Content = 0,
        };
    }

    private ResponseMessage<int> AddSubscription(
        Guid clientId,
        string? subscription)
    {
        if (string.IsNullOrWhiteSpace(subscription))
        {
            return ResponseMessage<int>.Empty;
        }

        bool response = _userService.AddSubscription(clientId, subscription);

        return new ResponseMessage<int>
        {
            Successs = response,
            Content = 0,
        };
    }
}