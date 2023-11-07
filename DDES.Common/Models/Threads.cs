using System.Text.Json.Serialization;

namespace DDES.Common.Models;

public sealed class Threads
{
    [JsonPropertyName("threads")]
    public List<Thread> ThreadList { get; set; } = new();
}