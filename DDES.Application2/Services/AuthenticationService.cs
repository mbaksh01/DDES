﻿using System.Diagnostics.CodeAnalysis;
using DDES.Application2.Services.Abstractions;
using DDES.Common.Enums;
using DDES.Common.Models;

namespace DDES.Application2.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IMessagingService _messagingService;

    public User? User { get; private set; }

    public AuthenticationService(IMessagingService messagingService)
    {
        _messagingService = messagingService;
    }

    public bool Authenticate(string username, string password,
        [MaybeNullWhen(false)] out User user)
    {
        var response = _messagingService.Send<User, User>(
            MessageType.Authenticate, new User
            {
                Username = username,
                Password = password,
                Roles = new List<string>(),
            });

        User = user = response.Content;
        return response.Successs;
    }
}