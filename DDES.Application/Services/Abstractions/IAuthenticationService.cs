using System.Diagnostics.CodeAnalysis;
using DDES.Common.Models;

namespace DDES.Application.Services.Abstractions;

public interface IAuthenticationService
{
    public User? User { get; }

    bool IsAuthenticated { get; }

    event Action<User> UserAuthenticated;

    bool Authenticate(
        string username,
        string password,
        [MaybeNullWhen(false)] out User user);
}