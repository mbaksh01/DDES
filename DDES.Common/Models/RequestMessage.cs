using DDES.Common.Enums;

namespace DDES.Common.Models;

public class RequestMessage<TData>
{
    public required Guid ClientId { get; init; }

    public MessageType MessageType { get; init; }

    public TData? Content { get; init; }

    public static readonly RequestMessage<TData> Empty =
        new()
        {
            ClientId = Guid.Empty,
            MessageType = MessageType.Unknown,
        };
}