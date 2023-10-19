namespace DDES.Data.Models;

public sealed record Client(
    Guid Id,
    byte[] PublicKey,
    string UserName
);
