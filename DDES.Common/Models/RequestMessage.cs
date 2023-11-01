using DDES.Common.Enums;

namespace DDES.Common.Models;

public class RequestMessage<TData>
{
    public MessageType MessageType { get; set; }

    public TData? Data { get; set; }

    public static readonly RequestMessage<TData> Empty =
        new()
        {
            MessageType = MessageType.Unknown,
        };
}