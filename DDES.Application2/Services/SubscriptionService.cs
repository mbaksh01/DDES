using NetMQ;
using NetMQ.Sockets;

namespace DDES.Application2.Services;

public class SubscriptionService
{
    public void Subscribe()
    {
        using SubscriberSocket subscriber = new();
        subscriber.Connect("tcp://127.0.0.1:5554");
        subscriber.Subscribe("Test");

        while (true)
        {
            var topic = subscriber.ReceiveFrameString();
            var msg = subscriber.ReceiveFrameString();
            Console.WriteLine("From Publisher: {0} {1}", topic, msg);
        }
    }
}