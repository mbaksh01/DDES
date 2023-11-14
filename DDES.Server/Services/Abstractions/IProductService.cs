using DDES.Common.Models;

namespace DDES.Server.Services.Abstractions;

public interface IProductService
{
    void AddProduct(Product product);

    List<Product> GetAllProducts();

    void UpdateProduct(Product product);

    void DeleteProduct(Product product);
}