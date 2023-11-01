using System.Text.Json.Serialization;

namespace DDES.Common.Models;

public class User
{
    [JsonPropertyName("username")] public required string Username { get; set; }

    [JsonPropertyName("password")] public required string Password { get; set; }

    [JsonPropertyName("roles")] public required List<string> Roles { get; set; }
}