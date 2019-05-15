using System;
using System.Linq;
using System.Threading.Tasks;
using JOS.TrafikLab.Client;
using JOS.TrafikLab.Core;
using JOS.TrafikLab.Functions.Infrastructure;
using Microsoft.WindowsAzure.Storage;

namespace JOS.TrafikLab.Functions.Features.StopPoints
{
    public class UpdateStopPointsBlobCommand : IUpdateStopPointsBlobCommand
    {
        private readonly TrafikLabApiClient _trafikLabApiClient;
        private readonly ISerializer _serializer;
        private readonly AzureSettings _azureSettings;

        public UpdateStopPointsBlobCommand(
            TrafikLabApiClient trafikLabApiClient,
            ISerializer serializer,
            AzureSettings azureSettings)
        {
            _trafikLabApiClient = trafikLabApiClient ?? throw new ArgumentNullException(nameof(trafikLabApiClient));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _azureSettings = azureSettings ?? throw new ArgumentNullException(nameof(azureSettings));
        }

        public async Task Execute()
        {
            var result = await _trafikLabApiClient.GetLineDataV1();
            var mapped = result.ResponseData.Result.Select(x => new StopPoint(
                x.StopPointName,
                new Location(x.LocationEastingCoordinate, x.LocationNorthingCoordinate),
                x.StopAreaNumber,
                GetStopAreaType(x.StopAreaTypeCode),
                x.ExistsFromDate)
            );

            if (CloudStorageAccount.TryParse(_azureSettings.Storage.ConnectionString, out var storageAccount))
            {
                var json = _serializer.Serialize(mapped);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var blobContainer = blobClient.GetContainerReference(_azureSettings.Storage.ContainerName);
                var blobReference = blobContainer.GetBlockBlobReference("StopPoints.json");
                blobReference.Properties.ContentType = "application/json";
                await blobReference.UploadTextAsync(json);
            }
        }

        private static StopAreaType GetStopAreaType(string stopAreaType)
        {
            switch (stopAreaType)
            {
                case "BUSTERM":
                    return StopAreaType.Bus;
                case "TRAMSTN":
                    return StopAreaType.Tram;
                case "METROSTN":
                    return StopAreaType.Metro;
                case "RAILWSTN":
                    return StopAreaType.Railway;
                case "SHIPBER":
                    return StopAreaType.Ship;
                case "FERRYBER":
                    return StopAreaType.Ferry;
                default:
                    return StopAreaType.Unknown;

            }
        }
    }
}
