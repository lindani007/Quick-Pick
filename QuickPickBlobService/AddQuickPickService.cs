using Microsoft.Extensions.DependencyInjection;
using QuickPickBlobService.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickPickBlobService
{
    public static class AddQuickPickService
    {
        public static IServiceCollection QuickPickBlobService(this IServiceCollection service, Action<ApiBaseUrl> config)
        {
            if (service == null) { throw new ArgumentNullException(nameof(service)); }
            if (config == null) { throw new ArgumentNullException(nameof(config)); }
            service.Configure(config);
            service.AddSingleton<ImageStorege>(provider =>
            {
                var options = provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<ApiBaseUrl>>().Value;
                if(!string.IsNullOrEmpty(options.BaseUrl))
                return new ImageStorege(options.BaseUrl);
                else
                    throw new ArgumentNullException("ApiBaseUrl.BaseUrl is null or empty");
            });
            return service;
        }
    }
}
