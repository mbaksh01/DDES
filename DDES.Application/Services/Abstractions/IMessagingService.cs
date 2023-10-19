using DDES.Data.Models;

namespace DDES.Application.Services.Abstractions;

public interface IMessagingService
{
    User? Authenticate(User user);

    string SendMessage(string data);
}