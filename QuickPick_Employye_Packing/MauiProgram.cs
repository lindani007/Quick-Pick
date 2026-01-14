using Microsoft.Extensions.Logging;
using QuickPickDBApiService;
using QuickPickSignlaRService.Services;

namespace QuickPick_Employye_Packing
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.Services.AddSingleton<SignalROrderService>();
            builder.Services.AddSingleton<TransactionService>();
            builder.Services.AddSingleton<SignalROrderdItemService>();
            builder.Services.AddSingleton<StatusService>();
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
