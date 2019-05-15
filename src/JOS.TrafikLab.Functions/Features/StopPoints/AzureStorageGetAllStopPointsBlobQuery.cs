using System;
using System.IO;
using System.Threading.Tasks;
using JOS.Core;
using JOS.TrafikLab.Core;
using JOS.TrafikLab.Functions.Infrastructure;
using Microsoft.WindowsAzure.Storage;

namespace JOS.TrafikLab.Functions.Features.StopPoints
{
    public class AzureStorageGetAllStopPointsBlobQuery : IGetAllStopPointsBlobQuery
    {
        private readonly AzureSettings _azureSettings;

        public AzureStorageGetAllStopPointsBlobQuery(AzureSettings azureSettings)
        {
            _azureSettings = azureSettings ?? throw new ArgumentNullException(nameof(azureSettings));
        }

        public async Task<Result<Stream>> Execute()
        {
            if (CloudStorageAccount.TryParse(_azureSettings.Storage.ConnectionString, out var storageAccount))
            {
                var cloudBlobClient = storageAccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(_azureSettings.Storage.ContainerName);
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference("StopPoints.json");
                var responseStream = new MemoryStream();
                await cloudBlockBlob.DownloadToStreamAsync(responseStream);

                responseStream.Position = 0;

                return Result<Stream>.Ok(responseStream);
            }

            return Result<Stream>.Fail();
        }
    }
}
