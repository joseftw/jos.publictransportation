using System.Linq;
using JOS.TrafikLab.Client;
using JOS.TrafikLab.Functions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(StartUp))]
namespace JOS.TrafikLab.Functions
{
    public class StartUp : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            var configurationBuilder = new ConfigurationBuilder();
            var iconfigurationDescriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(IConfiguration));
            if (iconfigurationDescriptor?.ImplementationInstance is IConfigurationRoot configuration)
            {
                configurationBuilder.AddConfiguration(configuration);
            }

            configurationBuilder.AddUserSecrets<StartUp>();
            configurationBuilder.AddEnvironmentVariables();
            configurationBuilder.AddInMemoryCollection();

            var config = configurationBuilder.Build();

            builder.Services.Remove(builder.Services.FirstOrDefault(x => x.ServiceType == typeof(IConfigurationRoot)));
            builder.Services.Remove(iconfigurationDescriptor);
            builder.Services.AddSingleton<IConfigurationRoot>(config);
            builder.Services.AddSingleton<IConfiguration>(config);
            builder.Services.AddTrafikLab();
        }
    }
}
