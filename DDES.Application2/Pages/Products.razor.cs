using DDES.Application2.Services.Abstractions;
using DDES.Common.Models;
using Microsoft.AspNetCore.Components;

namespace DDES.Application2.Pages;

public partial class Products : ComponentBase
{
    private List<Product> _products = new();

    [Inject] private IProductService ProductService { get; set; } = default!;

    protected override void OnInitialized()
    {
        _products = ProductService.GetAllProducts();
        base.OnInitialized();
    }
}