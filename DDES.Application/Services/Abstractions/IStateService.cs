using DDES.Common.Models;

namespace DDES.Application.Services.Abstractions;

public interface IStateService
{
    User? CurrentUser { get; set; }

    bool IsAuthenticated { get; }
}