using DDES.Application2.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace DDES.Application2.Shared;

public sealed partial class NavMenu : ComponentBase
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IAuthenticationService AuthenticationService { get; set; }
        = default!;

    private string _lastSelectedPage = "dashboard";

    private readonly Dictionary<string, bool> _selectedPage = new()
    {
        ["dashboard"] = true,
        ["products"] = false,
        ["notifications"] = false,
        ["messages"] = false,
        ["account"] = false,
        ["login"] = false,
    };

    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += LocationChanged;

        base.OnInitialized();
    }

    private void LocationChanged(object? obj, LocationChangedEventArgs args)
    {
        string page = NavigationManager
            .Uri[(NavigationManager.Uri.LastIndexOf('/') + 1)..];

        if (string.IsNullOrEmpty(page))
        {
            _selectedPage["dashboard"] = true;
            _selectedPage[_lastSelectedPage] = false;
            _lastSelectedPage = "dashboard";
        }
        else
        {
            _selectedPage[page] = true;
            _selectedPage[_lastSelectedPage] = false;
            _lastSelectedPage = page;
        }

        StateHasChanged();
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= LocationChanged;
    }
}