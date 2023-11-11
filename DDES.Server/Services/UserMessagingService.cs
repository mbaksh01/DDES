using System.Text.Json;
using DDES.Common.Models;
using Thread = DDES.Common.Models.Thread;

namespace DDES.Server.Services;

public class UserMessagingService
{
    private Threads _threads = new();

    public UserMessagingService()
    {
        LoadThreads();
    }

    private void LoadThreads()
    {
        if (_threads.ThreadList.Any())
        {
            return;
        }

        using StreamReader sr = new("Data/messages.json");

        string users = sr.ReadToEnd();

        _threads = JsonSerializer.Deserialize<Threads>(users) ??
                   new Threads();
    }

    public IEnumerable<Thread> GetThreads(string username)
    {
        return _threads
            .ThreadList
            .Where(t =>
                t.SupplierUsername.Equals(username,
                    StringComparison.OrdinalIgnoreCase)
                || t.CustomerUsername.Equals(username,
                    StringComparison.OrdinalIgnoreCase));
    }

    public void AddThreadMessage(
        string supplierUsername,
        string customerUsername,
        ThreadMessage message)
    {
        Thread? thread = _threads
            .ThreadList
            .FirstOrDefault(t =>
                t.SupplierUsername.Equals(supplierUsername,
                    StringComparison.OrdinalIgnoreCase)
                && t.CustomerUsername.Equals(customerUsername,
                    StringComparison.OrdinalIgnoreCase));

        thread?.Messages.Add(message);
    }
}