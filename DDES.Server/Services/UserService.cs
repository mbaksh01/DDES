using System.Text.Json;
using DDES.Common.Models;
using DDES.Server.Services.Abstractions;

namespace DDES.Server.Services;

internal class UserService : IUserService
{
    private readonly IClientService _clientService;
    private readonly ILogger<UserService> _logger;
    private List<User> _users = new();

    public UserService(
        IClientService clientService,
        ILogger<UserService> logger)
    {
        _clientService = clientService;
        _logger = logger;
        LoadUsers();
    }

    private void LoadUsers()
    {
        if (_users.Any())
        {
            return;
        }

        using StreamReader sr = new("Data/credentials.json");

        string users = sr.ReadToEnd();

        _users = JsonSerializer.Deserialize<List<User>>(users) ??
                 new List<User>();
    }

    private async Task SaveUsers()
    {
        await using StreamWriter sw = new("Data/credentials.json",
            append: false);

        sw.WriteLine(JsonSerializer.Serialize(_users));
    }

    public User? Authenticate(string username, string password)
    {
        LoadUsers();

        User? user = _users.FirstOrDefault(u => u.Username == username);

        return user is null
            ? null
            : user.Password != password
                ? null
                : user;
    }

    public bool AddSubscription(Guid clientId, string subscription)
    {
        string? username = _clientService.GetUsername(clientId);

        if (username is null)
        {
            return false;
        }

        User? user = _users.Find(u => u.Username == username);

        if (user is null)
        {
            return false;
        }

        user.Subscriptions.Add(subscription);

        _logger.LogInformation(
            "Added subscription: {subscription} for user: {username}",
            subscription, username);

        return true;
    }
}