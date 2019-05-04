using System;
using Microsoft.Extensions.DependencyInjection;

namespace JOS.TrafikLab.Client
{
    public class TrafikLabClientFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public TrafikLabClientFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TrafikLabApiClient Create()
        {
            return _serviceProvider.GetRequiredService<TrafikLabApiClient>();
        }
    }
}
