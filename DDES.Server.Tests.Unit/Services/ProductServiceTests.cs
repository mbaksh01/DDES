using DDES.Server.Services.Abstractions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;

namespace DDES.Server.Tests.Unit.Services;

public class ProductServiceTests
{
    private readonly ILogger<ProductService> _logger =
        Substitute.For<ILogger<ProductService>>();

    private readonly IProductService _sut;

    public ProductServiceTests()
    {
        _sut = new ProductService(_logger);
    }

    [Fact]
    public void Product_Service_Should_Load_Existing_Products_On_Construction()
    {
        // Act
        List<Product> products = _sut.GetAllProducts();

        // Assert
        products.Should().NotBeEmpty();
    }

    [Fact]
    public void Update_Product_Should_Update_A_Products_Values()
    {
        // Arrange
        Product product = new()
        {
            Id = Guid.NewGuid(),
            Name = "TestProduct",
            Description = "Test product",
            Price = 20,
            Seller = "TestSeller",
        };

        _sut.AddProduct(product);

        product.Name = "Test Product";
        product.Description = "More product information";
        product.Price = 40;
        product.Seller = "TestSeller2";

        // Act
        _sut.UpdateProduct(product);

        // Assert
        Product? updatedProduct = _sut.GetAllProducts().FirstOrDefault(
            p => p.Id == product.Id);

        updatedProduct.Should().NotBeNull();

        using (new AssertionScope())
        {
            updatedProduct!.Name.Should().Be(product.Name);
            updatedProduct.Description.Should().Be(product.Description);
            updatedProduct.Price.Should().Be(product.Price);
            updatedProduct.Seller.Should().Be(product.Seller);
        }
    }

    [Fact]
    public void Delete_Product_Should_Delete_A_Product()
    {
        // Arrange
        Product product = _sut.GetAllProducts().First();

        // Act
        _sut.DeleteProduct(product);

        // Assert
        Product? notFoundProduct = _sut.GetAllProducts()
            .FirstOrDefault(p => p.Id == product.Id);

        notFoundProduct.Should().BeNull();
    }
}