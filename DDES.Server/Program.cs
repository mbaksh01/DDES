using DDES.Common.Enums;
using DDES.Server.Services;
using DDES.Server.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHostBuilder builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services =>
{
    _ = services.AddSingleton<IClientService, ClientService>();
    _ = services.AddSingleton<IUserService, UserService>();
    _ = services.AddSingleton<MessagingService>();
    _ = services.AddSingleton<PublishingService>();
    _ = services.AddSingleton<UserMessagingService>();
    _ = services.AddSingleton<IProductService, ProductService>();
});

IHost app = builder.Build();

PublishingService publishingService =
    app.Services.GetRequiredService<PublishingService>();

Thread notifications =
    new(() =>
    {
        while (true)
        {
            publishingService.PublishMessage(Topics.CustomerNotification,
                "Hello World");
            Thread.Sleep(1000);
        }
    });
notifications.Start();

MessagingService messagingService =
    app.Services.GetRequiredService<MessagingService>();

app.Start();

messagingService.Listen();