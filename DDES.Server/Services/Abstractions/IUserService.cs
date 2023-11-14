using DDES.Common.Models;

namespace DDES.Server.Services.Abstractions;

internal interface IUserService
{
    User? Authenticate(string username, string password);

    bool AddSubscription(Guid clientId, string subscription);
}