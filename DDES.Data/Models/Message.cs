using DDES.Data.Enums;

namespace DDES.Data.Models;

public sealed record Message(Guid ClientId, MessageType MessageType, string? Data)
{
    public static Message CreateResponseMessage(string? data)
    {
        return new Message(Guid.Empty, MessageType.Unknown, data);
    }
}