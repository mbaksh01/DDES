using System.Text.Json.Serialization;

namespace DDES.Common.Models;

public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public float Price { get; set; }

    [JsonPropertyName("sellerUsername")]
    public string Seller { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}