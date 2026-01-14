using Microsoft.Extensions.Logging;
using QuickPickDBApiService;
using QuickPickDBApiService.Services;
using QuickPickSignlaRService.Services;

namespace Order_Distribution
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.Services.AddSingleton<OrderService>();
            builder.Services.AddSingleton<SignalROrderdItemService>();
            builder.Services.AddSingleton<SignalROrderService>();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddQuickPickDbApiServices(url => url.BaseUrl = "https://localhost:7148");
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
