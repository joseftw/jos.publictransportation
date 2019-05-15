using System;
using System.Threading.Tasks;
using JOS.TrafikLab.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace JOS.TrafikLab.Functions.Features.StopPoints
{
    public class UpdateStopPointsFunction
    {
        private readonly IUpdateStopPointsBlobCommand _updateStopPointsBlobCommand;

        public UpdateStopPointsFunction(IUpdateStopPointsBlobCommand updateStopPointsBlobCommand)
        {
            _updateStopPointsBlobCommand = updateStopPointsBlobCommand ?? throw new ArgumentNullException(nameof(updateStopPointsBlobCommand));
        }

        /// <summary>
        ///  Fetches all Stations/Bus Stops and stores it
        ///  Runs daily at 03:00 UTC
        /// </summary>
        /// <param name="myTimer"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName(nameof(UpdateStopPointsFunction))]
        public async Task Run([TimerTrigger("0 0 03 * * *")]TimerInfo myTimer, ILogger log)
        {
            await _updateStopPointsBlobCommand.Execute();
        }
    }
}
