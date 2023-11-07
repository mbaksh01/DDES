using DDES.Common.Enums;

namespace DDES.Common.Models;

public class RequestMessage<TData>
{
    public required Guid ClientId { get; set; }

    public MessageType MessageType { get; set; }

    public TData? Data { get; set; }

    public static readonly RequestMessage<TData> Empty =
        new()
        {
            ClientId = Guid.Empty,
            MessageType = MessageType.Unknown,
        };
}