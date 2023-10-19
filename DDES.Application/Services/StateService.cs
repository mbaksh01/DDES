using DDES.Application.Services.Abstractions;
using DDES.Data.Models;

namespace DDES.Application.Services;

internal sealed class StateService : IStateService
{
    public User? CurrentUser { get; set; }

    public bool IsAuthenticated => CurrentUser is not null;
}
