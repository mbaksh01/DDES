using DDES.Data.Models;
using DDES.Server.Services.Abstractions;
using System.Text.Json;

namespace DDES.Server.Services;

internal class UserService : IUserService
{
    private List<User> _users = new();

    public UserService()
    {
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

        _users = JsonSerializer.Deserialize<List<User>>(users) ?? new List<User>();
    }

    private async Task SaveUsers()
    {
        await using StreamWriter sw = new("Data/credentials.json", append: false);

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
}
