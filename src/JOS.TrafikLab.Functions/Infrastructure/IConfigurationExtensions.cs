using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace JOS.TrafikLab.Functions.Infrastructure
{
    /// <summary>
    /// Ugly hack for Azure Functions environment variables that does not work that well with double underscores and stuff
    /// It's easier to just read azure settings with .GetEnvironmentVariable instead of reading it from IConfiguration.
    /// IConfiguration is used when developing locally to allow the usage of UserSecrets.
    /// </summary>
    public static class IConfigurationExtensions
    {
        public static T Get<T>(this IConfiguration configuration, string key, object fallbackValue = null)
        {
            var configValue = configuration.GetValue<T>(key);

            if (configValue != null)
            {
                return configValue;
            }

            var sections = key.Split(':');
            var environmentVariableKey = sections.Length > 1 ? $"{string.Join("__", sections.Take(sections.Length - 1))}_{sections.Last()}" : key;
            var environmentVariable = Environment.GetEnvironmentVariable(environmentVariableKey);

            if (environmentVariable != null)
            {
                return (T)Convert.ChangeType(environmentVariable, typeof(T));
            }

            if (fallbackValue != null)
            {
                return (T)fallbackValue;
            }

            throw new Exception($"Missing config for key '{key}'");
        }
    }
}
