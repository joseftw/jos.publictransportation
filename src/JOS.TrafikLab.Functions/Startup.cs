using JOS.TrafikLab.Client;
using JOS.TrafikLab.Core;
using JOS.TrafikLab.Functions;
using JOS.TrafikLab.Functions.Features.StopPoints;
using JOS.TrafikLab.Functions.Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

[assembly: FunctionsStartup(typeof(Startup))]
namespace JOS.TrafikLab.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = SetupConfiguration(builder.Services);

            builder.Services.AddSingleton<IUpdateStopPointsBlobCommand, UpdateStopPointsBlobCommand>();

            AddSerializer(builder.Services);
            AddAzureStorage(config, builder.Services);
            AddTrafikLab(config, builder.Services);
        }

        private static IConfiguration SetupConfiguration(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            configurationBuilder.AddConfiguration(configuration);
            configurationBuilder.AddUserSecrets<Startup>();
            var config = configurationBuilder.Build();
            services.Replace(ServiceDescriptor.Singleton(typeof(IConfiguration), config));

            return config;
        }

        private static void AddSerializer(IServiceCollection services)
        {
            var contractResolver = new CamelCasePropertyNamesContractResolver();
            services.AddSingleton(new JsonSerializer
            {
                ContractResolver = contractResolver
            });
            services.AddSingleton<IUpdateStopPointsBlobCommand, UpdateStopPointsBlobCommand>();
            services.AddSingleton<ISerializer, NewtonsoftSerializer>();
        }

        private static void AddTrafikLab(IConfiguration config, IServiceCollection services)
        {
            var realTimeDeparturesApiKey = config.Get<string>("TrafikLab:RealTimeDepartures:V4:ApiKey");
            var lineDataApiKey = config.Get<string>("TrafikLab:LineData:V1:ApiKey");
            var defaultTimeoutInMs = config.Get<int?>("TrafikLab:DefaultTimeoutInMs", 5000);

            var trafikLabSettings = new TrafikLabSettings(
                realTimeDeparturesApiKey,
                lineDataApiKey,
                defaultTimeoutInMs.Value);

            services.AddSingleton(trafikLabSettings);
            services.AddTrafikLab(trafikLabSettings);
        }

        private static void AddAzureStorage(IConfiguration config, IServiceCollection services)
        {
            var connectionStringPath = "Azure:Storage:ConnectionString";
            var containerNamePath = "Azure:Storage:ContainerName";
            var connectionString = config.Get<string>(connectionStringPath);
            var containerName = config.Get<string>(containerNamePath, "location-data");
            var storageSettings = new StorageSettings(connectionString, containerName);
            var azureSettings = new AzureSettings(storageSettings);
            services.AddSingleton(azureSettings);
        }
    }
}
