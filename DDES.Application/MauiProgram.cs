﻿using DDES.Application.Services;
using DDES.Application.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace DDES.Application;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services
            .AddSingleton<IAuthenticationService, AuthenticationService>()
            .AddSingleton<IMessagingService, MessagingService>()
            .AddSingleton<IClientService, ClientService>()
            .AddSingleton<IUserMessagingService, UserMessagingService>()
            .AddSingleton<ISubscriptionService, SubscriptionService>()
            .AddSingleton<IProductService, ProductService>();

        return builder.Build();
    }
}