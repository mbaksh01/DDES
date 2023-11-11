namespace DDES.Common.Models;

public sealed record ThreadMessageRequest
{
    public required string CustomerUsername { get; set; }

    public required string SupplierUsername { get; set; }

    public required ThreadMessage Message { get; set; }
}