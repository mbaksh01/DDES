using DDES.Data.Models;

namespace DDES.Server.Services.Abstractions;

internal interface IUserService
{
    User? Authenticate(string username, string password);
}
