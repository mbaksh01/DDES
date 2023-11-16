using DDES.Application.Services.Abstractions;
using DDES.Common.Models;
using Microsoft.AspNetCore.Components;

namespace DDES.Application.Pages;

public partial class Products : ComponentBase
{
    private List<Product> _products = new();

    [Inject] private IProductService ProductService { get; set; } = default!;

    protected override void OnInitialized()
    {
        _products = ProductService.GetAllProducts();
        base.OnInitialized();
    }

    private void SortChanged(ChangeEventArgs args)
    {
        _products = (args.Value as string) switch
        {
            "name" => _products.OrderBy(p => p.Name).ToList(),
            "priceHighest" => _products.OrderByDescending(p => p.Price)
                .ToList(),
            "priceLowest" => _products.OrderBy(p => p.Price).ToList(),
            _ => _products,
        };
    }
}