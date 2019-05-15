using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using JOS.TrafikLab.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace JOS.TrafikLab.Functions.Features.StopPoints
{
    public class GetAllStopPointsFunction
    {
        private readonly IGetAllStopPointsBlobQuery _getAllStopPointsBlobQuery;

        public GetAllStopPointsFunction(IGetAllStopPointsBlobQuery getAllStopPointsBlobQuery)
        {
            _getAllStopPointsBlobQuery = getAllStopPointsBlobQuery ?? throw new ArgumentNullException(nameof(getAllStopPointsBlobQuery));
        }

        [FunctionName(nameof(GetAllStopPointsFunction))]
        public async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest request, ILogger log)
        {
            var result = await _getAllStopPointsBlobQuery.Execute();

            if (result.Success)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(result.Data)
                };
                response.Headers.CacheControl = new CacheControlHeaderValue
                {
                    MaxAge = TimeSpan.FromHours(12),
                    Public = true,
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);
                return response;
            }

            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}
