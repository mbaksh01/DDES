using DDES.Common.Models;

namespace DDES.Application2.Services.Abstractions;

public interface IUserMessagingService
{
    void SendMessage(
        string supplierName,
        string customerName,
        ThreadMessage message);
}