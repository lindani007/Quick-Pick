using Microcharts.Maui;
using Microsoft.Extensions.Logging;
using QuickPickBlobService;
using QuickPickDBApiService;
using QuickPickSignlaRService.Services;
using Microsoft.Maui.Devices;

namespace QuickPick_Employer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.Services.AddSingleton<SignlaRAisleService>();
            builder.Services.AddSingleton<SignalROrderService>();
            builder.Services.AddSingleton<TransactionService>();
            builder.Services.AddSingleton<SignalROrderdItemService>();

            builder
                .UseMauiApp<App>()
                .UseMicrocharts()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.QuickPickBlobService(url => url.BaseUrl = "https://localhost:7279");
            builder.Services.AddQuickPickDbApiServices(url => url.BaseUrl = "https://localhost:7148");

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
