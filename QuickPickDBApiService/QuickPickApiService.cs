using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using QuickPickDBApiService.Models;
using QuickPickDBApiService.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickPickDBApiService
{
    public static class QuickPickApiService
    {
        public static IServiceCollection AddQuickPickDbApiServices(this IServiceCollection services, Action<ApiBaseUrl> configure)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            // Configure ApiBaseUrl in the options system
            services.Configure(configure);

            // Register concrete services that require ApiBaseUrl in their constructors.
            services.AddSingleton<AisleService>(provider =>
            {
                var apiBaseUrl = provider.GetRequiredService<IOptions<ApiBaseUrl>>().Value;
                return new AisleService(apiBaseUrl);
            });

            services.AddSingleton<ItemService>(provider =>
            {
                var apiBaseUrl = provider.GetRequiredService<IOptions<ApiBaseUrl>>().Value;
                return new ItemService(apiBaseUrl);
            });

            services.AddSingleton<OrderService>(provider =>
            {
                var apiBaseUrl = provider.GetRequiredService<IOptions<ApiBaseUrl>>().Value;
                return new OrderService(apiBaseUrl);
            });

            // Keep original name 'Sles' (typo preserved) — change if your service type differs.
            services.AddSingleton<SlesService>(provider =>
            {
                var apiBaseUrl = provider.GetRequiredService<IOptions<ApiBaseUrl>>().Value;
                return new SlesService(apiBaseUrl);
            });

           services.AddSingleton<DbBoughtItemService>(provider =>
            {
                var apiBaseUrl =  provider.GetRequiredService<IOptions<ApiBaseUrl>>().Value;
                return new DbBoughtItemService(apiBaseUrl);
            });
            services.AddSingleton<StockService>(provider =>
            {
                var apiBaseUrl = provider.GetRequiredService<IOptions<ApiBaseUrl>>().Value;
                return new StockService(apiBaseUrl);
            });
            services.AddSingleton<LoginService>(provider =>
            {
                var apiBaseUrl = provider.GetRequiredService<IOptions<ApiBaseUrl>>().Value;
                return new LoginService(apiBaseUrl);
            });
            return services;
        }
    }
    }
