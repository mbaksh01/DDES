﻿using System.Text.Json;
using DDES.Common.Enums;
using DDES.Common.Models;
using DDES.Server.Services.Abstractions;
using Thread = DDES.Common.Models.Thread;

namespace DDES.Server.Services;

public class UserMessagingService : IUserMessagingService
{
    private Threads _threads = new();
    private readonly IPublishingService _publishingService;

    public UserMessagingService(IPublishingService publishingService)
    {
        _publishingService = publishingService;
        LoadThreads();
    }

    private void LoadThreads()
    {
        if (_threads.ThreadList.Any())
        {
            return;
        }

        using StreamReader sr = new("Data/messages.json");

        string threads = sr.ReadToEnd();

        _threads = JsonSerializer.Deserialize<Threads>(threads) ??
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

        _publishingService.PublishMessage(Topics.NewDirectMessage, message);
    }
}