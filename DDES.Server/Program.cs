using DDES.Server.Services;
using DDES.Server.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHostBuilder builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services =>
{
    _ = services.AddSingleton<IClientService, ClientService>();
    _ = services.AddSingleton<IUserService, UserService>();
    _ = services.AddSingleton<IMessagingService, MessagingService>();
    _ = services.AddSingleton<IPublishingService, PublishingService>();
    _ = services.AddSingleton<IUserMessagingService, UserMessagingService>();
    _ = services.AddSingleton<IProductService, ProductService>();
});

IHost app = builder.Build();

IPublishingService publishingService =
    app.Services.GetRequiredService<IPublishingService>();

Thread notifications =
    new(() =>
    {
        while (true)
        {
            publishingService.PublishMessage("test",
                "Hello World");
            Thread.Sleep(1000);
        }
    });
// notifications.Start();

IMessagingService messagingService =
    app.Services.GetRequiredService<IMessagingService>();

app.Start();

messagingService.Listen();