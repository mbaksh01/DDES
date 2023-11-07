using System.Text.Json.Serialization;

namespace DDES.Common.Models;

public sealed class Thread
{
    [JsonPropertyName("supplierUsername")]
    public string SupplierUsername { get; set; } = string.Empty;

    [JsonPropertyName("customerUsername")]
    public string CustomerUsername { get; set; } = string.Empty;

    [JsonPropertyName("messages")]
    public List<ThreadMessage> Messages { get; set; } = new();
}