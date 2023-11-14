using DDES.Application2.Services.Abstractions;
using DDES.Common.Enums;
using DDES.Common.Models;
using Microsoft.AspNetCore.Components;

namespace DDES.Application2.Pages;

public partial class Account : ComponentBase
{
    private bool _showCreateProduct;
    private bool _updateExistingProduct;
    private bool _deleteProduct;
    private bool _showBroadcastMessage;
    private bool _showSubscriptions;

    private List<Product> _products = new();

    private Product? _selectedProduct;

    private string _broadcastMessage = string.Empty;

    private string _selectedSubscription = string.Empty;

    [Inject]
    private IAuthenticationService AuthenticationService { get; set; }
        = default!;

    [Inject] private IProductService ProductService { get; set; } = default!;

    [Inject]
    private IMessagingService MessagingService { get; set; } = default!;

    [Inject]
    private ISubscriptionService SubscriptionService { get; set; } = default!;

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

    private void AccountPanelBroadcastMessage()
    {
        _showBroadcastMessage = true;
        _broadcastMessage = string.Empty;
    }

    private void AccountPanelSubscribe()
    {
        _showSubscriptions = true;
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

    private void BroadcastMessage()
    {
        if (string.IsNullOrEmpty(_broadcastMessage))
        {
            return;
        }

        MessagingService.Send<string, int>(MessageType.BroadcastMessage,
            _broadcastMessage);
    }

    private void AddSubscription()
    {
        if (string.IsNullOrWhiteSpace(_selectedSubscription))
        {
            return;
        }

        string internalSubscriptionName = _selectedSubscription switch
        {
            "Customer Notifications" => Topics.CustomerNotification,
            "Personal Notifications" => Topics.PersonalNotification,
            _ => Topics.GeneralNotification,
        };

        SubscriptionService.AddSubscription(internalSubscriptionName);
    }
}