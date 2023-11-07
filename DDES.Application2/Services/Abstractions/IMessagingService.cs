using DDES.Common.Enums;
using DDES.Common.Models;

namespace DDES.Application2.Services.Abstractions;

public interface IMessagingService
{
    ResponseMessage<TResponse> Send<TModel, TResponse>(MessageType messageType,
        TModel data);
}