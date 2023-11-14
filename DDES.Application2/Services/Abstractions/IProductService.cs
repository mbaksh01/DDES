using DDES.Common.Models;

namespace DDES.Application2.Services.Abstractions;

public interface IProductService
{
    void AddProduct(Product product);

    List<Product> GetAllProducts();

    void AddProducts(List<Product> products);

    void UpdateProduct(Product product);

    void DeleteProduct(Product product);
}