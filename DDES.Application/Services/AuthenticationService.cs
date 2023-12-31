﻿using System.Diagnostics.CodeAnalysis;
using DDES.Application.Services.Abstractions;
using DDES.Common.Enums;
using DDES.Common.Models;

namespace DDES.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IMessagingService _messagingService;

    public User? User { get; private set; }

    public bool IsAuthenticated => User is not null;

    public event Action<User>? UserAuthenticated;

    public AuthenticationService(IMessagingService messagingService)
    {
        _messagingService = messagingService;
    }

    public bool Authenticate(
        string username,
        string password,
        [MaybeNullWhen(false)] out User user)
    {
        var response = _messagingService.Send<User, User>(
            MessageType.Authenticate, new User
            {
                Username = username,
                Password = password,
                Roles = new List<string>(),
                Subscriptions = new List<string>(),
            });

        User = user = response.Content;

        if (response.Successs)
        {
            UserAuthenticated?.Invoke(user!);
        }

        return response.Successs;
    }
}