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
});

IHost app = builder.Build();

MessagingService messagingService =
    app.Services.GetRequiredService<MessagingService>();

app.Start();

messagingService.Listen();