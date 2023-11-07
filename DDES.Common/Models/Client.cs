namespace DDES.Common.Models;

public sealed record Client(
    Guid Id,
    string Username,
    int Port
);