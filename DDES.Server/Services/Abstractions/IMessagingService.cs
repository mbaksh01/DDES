namespace DDES.Server.Services.Abstractions;

internal interface IMessagingService
{
    void Listen(int port = 5555);
}