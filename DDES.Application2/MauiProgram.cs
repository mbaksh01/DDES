using DDES.Application2.Services;
using DDES.Application2.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace DDES.Application2;

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
            .AddSingleton<IClientService, ClientService>();

        return builder.Build();
    }
}