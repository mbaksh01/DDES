using System.Text.Json;
using DDES.Common.Models;
using DDES.Server.Services.Abstractions;

namespace DDES.Server.Services;

public class ProductService : IProductService
{
    private readonly ILogger<ProductService> _logger;
    private List<Product> _products = new();

    public ProductService(ILogger<ProductService> logger)
    {
        _logger = logger;
        LoadProducts();
    }

    private void LoadProducts()
    {
        if (_products.Any())
        {
            return;
        }

        using StreamReader sr = new("Data/products.json");

        string products = sr.ReadToEnd();

        _products = JsonSerializer.Deserialize<List<Product>>(
                        products,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                        })
                    ?? new List<Product>();

        _logger.LogInformation("Successfully loaded all products.");
    }

    public void AddProduct(Product product)
    {
        _products.Add(product);
    }

    public void UpdateProduct(Product product)
    {
        Product? existingProduct =
            _products.Find(p => p.Id == product.Id)
            ?? _products.Find(p => p.Name == product.Name);

        if (existingProduct is null)
        {
            return;
        }

        _products.Remove(existingProduct);
        _products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        Product? existingProduct =
            _products.Find(p => p.Id == product.Id)
            ?? _products.Find(p => p.Name == product.Name);

        if (existingProduct is null)
        {
            return;
        }

        _products.Remove(existingProduct);
    }

    public List<Product> GetAllProducts()
    {
        return _products;
    }
}