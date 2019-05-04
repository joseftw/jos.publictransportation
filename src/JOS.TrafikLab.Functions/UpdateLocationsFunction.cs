using System;
using System.Threading.Tasks;
using JOS.TrafikLab.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace JOS.TrafikLab.Functions
{
    public class UpdateLocationsFunction
    {
        private readonly GetRealTimeDeparturesQuery _getRealTimeDeparturesQuery;

        public UpdateLocationsFunction(GetRealTimeDeparturesQuery getRealTimeDeparturesQuery)
        {
            _getRealTimeDeparturesQuery = getRealTimeDeparturesQuery ?? throw new ArgumentNullException(nameof(getRealTimeDeparturesQuery));
        }

        [FunctionName(nameof(UpdateLocationsFunction))]
        public async Task Run([TimerTrigger("*/10 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            if (!myTimer.IsPastDue)
            {
                log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            }

            var hej = await _getRealTimeDeparturesQuery.Execute(9288);
        }
    }
}
