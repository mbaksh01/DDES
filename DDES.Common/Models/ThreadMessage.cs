using System.Text.Json.Serialization;

namespace DDES.Common.Models;

public class ThreadMessage
{
    [JsonPropertyName("message")]
    public string MessageText { get; set; } = string.Empty;

    [JsonPropertyName("from")] public string From { get; set; } = string.Empty;

    [JsonPropertyName("to")] public string To { get; set; } = string.Empty;

    [JsonPropertyName("dateTimeSent")]
    public DateTime DateTimeSent { get; set; }
}