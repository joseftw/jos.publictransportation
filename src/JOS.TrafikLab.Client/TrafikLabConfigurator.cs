using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JOS.TrafikLab.Client
{
    public static class TrafikLabConfigurator
    {
        public static void AddTrafikLab(this IServiceCollection services)
        {
            services.AddHttpClient<TrafikLabApiClient>(x => { x.BaseAddress = new Uri(TrafikLabConstants.ApiBaseUrl); });
            services.AddSingleton<TrafikLabClientFactory>();
            services.AddSingleton<GetRealTimeDeparturesQuery>();
            services.AddSingleton<TrafikLabSettings>(x =>
            {
                var config = x.GetRequiredService<IConfiguration>();
                var apiKeyPath = "TrafikLab:ApiKey";
                var apiKey = config.GetValue<string>(apiKeyPath) ?? throw new Exception($"No API key has been configured on the following path {apiKeyPath}");
                return new TrafikLabSettings(apiKey);
            });
        }
    }
}
