using System.Text.Json;
using DDES.Common.Enums;
using DDES.Common.Models;
using DDES.Server.Hosting;
using DDES.Server.Services;
using DDES.Server.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Thread = System.Threading.Thread;

IHostBuilder builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services =>
{
    _ = services.AddSingleton<IClientService, ClientService>();
    _ = services.AddSingleton<IUserService, UserService>();
    _ = services.AddSingleton<IMessagingService, MessagingService>();
    _ = services.AddSingleton<IPublishingService, PublishingService>();
    _ = services.AddSingleton<IUserMessagingService, UserMessagingService>();
    _ = services.AddSingleton<IProductService, ProductService>();

    _ = services.AddHostedService<InitializationService>();
});

IHost app = builder.Build();

IPublishingService publishingService =
    app.Services.GetRequiredService<IPublishingService>();

Thread notifications =
    new(() =>
    {
        ThreadMessage message = new()
        {
            From = "supplier",
            To = "customer",
            MessageText = "Hello World",
        };

        var serialized = JsonSerializer.Serialize(message);
        
        while (true)
        {
            publishingService.PublishMessage(Topics.NewDirectMessage, serialized);
            Thread.Sleep(1000);
        }
    });
notifications.Start();

app.Run();