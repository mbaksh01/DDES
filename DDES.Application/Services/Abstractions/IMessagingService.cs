using DDES.Common.Enums;
using DDES.Common.Models;

namespace DDES.Application.Services.Abstractions;

public interface IMessagingService
{
    int Port { get; }
    
    ResponseMessage<TResponse> Send<TModel, TResponse>(
        MessageType messageType,
        TModel data);
}