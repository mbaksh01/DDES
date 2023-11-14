using DDES.Application2.Services.Abstractions;
using DDES.Common.Models;
using Microsoft.AspNetCore.Components;

namespace DDES.Application2.Pages;

public partial class Account : ComponentBase
{
    private bool _showCreateProduct;
    private bool _updateExistingProduct;
    private bool _deleteProduct;

    private List<Product> _products = new();

    private Product? _selectedProduct;

    [Inject]
    private IAuthenticationService AuthenticationService { get; set; }
        = default!;

    [Inject] private IProductService ProductService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private void AddProduct()
    {
        _products.Add(new Product());
    }

    private void AccountPanelSellProductClicked()
    {
        _showCreateProduct = true;

        if (_products.Count == 0)
        {
            _products.Add(new Product());
        }
    }

    private void AccountPanelUpdateProductClicked()
    {
        _updateExistingProduct = true;

        if (_products.Count == 0)
        {
            _products.AddRange(ProductService.GetAllProducts());
        }

        _selectedProduct = _products.First();
    }

    private void AccountPanelDeleteProductClicked()
    {
        _deleteProduct = true;

        if (_products.Count == 0)
        {
            _products.AddRange(ProductService.GetAllProducts());
        }

        _selectedProduct = _products.First();
    }

    private void ProductSelectedChanged(ChangeEventArgs args)
    {
        string? productName = args.Value as string;

        if (string.IsNullOrWhiteSpace(productName))
        {
            return;
        }

        _selectedProduct = _products.FirstOrDefault(p => p.Name == productName);
    }

    private void SellProducts()
    {
        ProductService.AddProducts(_products);
        NavigationManager.NavigateTo("/products");
    }

    private void UpdateProduct()
    {
        if (_selectedProduct is null)
        {
            return;
        }

        ProductService.UpdateProduct(_selectedProduct);
    }

    private void DeleteProduct()
    {
        if (_selectedProduct is null)
        {
            return;
        }

        ProductService.DeleteProduct(_selectedProduct);
    }
}