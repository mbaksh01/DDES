using DDES.Application.Services.Abstractions;
using DDES.Common.Enums;
using DDES.Common.Models;

namespace DDES.Application.Services;

public class ProductService : IProductService
{
    private readonly IMessagingService _messagingService;
    private readonly IAuthenticationService _authenticationService;
    private List<Product> _products = new();

    public ProductService(
        IMessagingService messagingService,
        IAuthenticationService authenticationService)
    {
        _messagingService = messagingService;
        _authenticationService = authenticationService;
    }

    public void AddProduct(Product product)
    {
        product.Id = Guid.NewGuid();

        product.Seller = _authenticationService.User?.Username ?? string.Empty;

        _messagingService.Send<Product, Product>(MessageType.AddProduct,
            product);

        _products.Add(product);
    }

    public void AddProducts(List<Product> products)
    {
        foreach (Product product in products)
        {
            AddProduct(product);
        }
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

        _messagingService.Send<Product, Product>(MessageType.UpdateProduct,
            product);

        _products.Remove(existingProduct);
        _products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        _messagingService.Send<Product, Product>(MessageType.DeleteProduct,
            product);

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
        ResponseMessage<List<Product>> response = _messagingService
            .Send<int, List<Product>>(MessageType.GetProducts, 0);

        if (response.Successs == false)
        {
            return _products;
        }

        _products.AddRange(response.Content!);

        return _products = _products
            .DistinctBy(p => p.Id)
            .OrderBy(p => p.Name)
            .ToList();
    }
}