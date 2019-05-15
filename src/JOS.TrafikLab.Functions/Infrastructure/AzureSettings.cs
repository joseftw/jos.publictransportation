using System;

namespace JOS.TrafikLab.Functions.Infrastructure
{
    public class AzureSettings
    {
        public StorageSettings Storage { get; }

        public AzureSettings(StorageSettings storageSettings)
        {
            Storage = storageSettings ?? throw new ArgumentNullException(nameof(storageSettings));
        }
    }

    public class StorageSettings
    {
        public StorageSettings(string connectionString, string containerName)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            ContainerName = containerName ?? throw new ArgumentNullException(nameof(containerName));
        }

        public string ConnectionString { get; }
        public string ContainerName { get; }
    }
}
