using DDES.Common.Models;

namespace DDES.Application.Services.Abstractions;

public interface IUserMessagingService
{
    void SendMessage(
        string supplierName,
        string customerName,
        ThreadMessage message);
}