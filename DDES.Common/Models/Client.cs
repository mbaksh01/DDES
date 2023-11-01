namespace DDES.Common.Models;

public sealed record Client(
    Guid Id,
    byte[] PublicKey,
    string UserName
);