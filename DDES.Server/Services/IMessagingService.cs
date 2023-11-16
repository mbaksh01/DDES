namespace DDES.Server.Services;

internal interface IMessagingService
{
    void Listen(int port = 5555);
}