using System;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JOS.TrafikLab.Client
{
    public static class TrafikLabConfigurator
    {
        public static void AddTrafikLab(this IServiceCollection services, TrafikLabSettings trafikLabSettings)
        {
            var contractResolver = new CamelCasePropertyNamesContractResolver();
            services.AddSingleton(new JsonSerializer()
            {
                ContractResolver = contractResolver
            });
            services.AddSingleton<IJsonDeserializer, JsonDeserializer>();
            services.AddHttpClient<TrafikLabApiClient>(x =>
            {
                x.BaseAddress = new Uri(TrafikLabConstants.ApiBaseUrl);
                x.Timeout = TimeSpan.FromMilliseconds(trafikLabSettings.DefaultTimeoutMs);
            });
            services.AddSingleton<TrafikLabClientFactory>();
        }

    }
}
