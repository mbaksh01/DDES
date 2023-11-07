namespace DDES.Common.Models;

public class ClientConnectedRequest
{
    public int Port { get; set; }

    public required string Username { get; set; }
}