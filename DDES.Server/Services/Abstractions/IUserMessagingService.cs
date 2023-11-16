using DDES.Common.Models;
using Thread = DDES.Common.Models.Thread;

namespace DDES.Server.Services.Abstractions;

public interface IUserMessagingService
{
    IEnumerable<Thread> GetThreads(string username);

    void AddThreadMessage(
        string supplierUsername,
        string customerUsername,
        ThreadMessage message);
}