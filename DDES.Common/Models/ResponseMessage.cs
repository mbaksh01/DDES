namespace DDES.Common.Models;

public class ResponseMessage<TResponse>
{
    public bool Successs { get; set; }

    public TResponse? Content { get; set; }

    public static readonly ResponseMessage<TResponse> Empty = new()
    {
        Successs = false,
    };
}