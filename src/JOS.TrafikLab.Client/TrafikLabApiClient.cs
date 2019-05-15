using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using JOS.TrafikLab.Client.LineData;
using Polly;

namespace JOS.TrafikLab.Client
{
    public class TrafikLabApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly TrafikLabSettings _trafikLabSettings;
        private readonly IJsonDeserializer _jsonDeserializer;
        private const string ApiPath = "/api2";
        private readonly AsyncPolicy _circuitBreakerPolicy;

        public TrafikLabApiClient(
            HttpClient httpClient,
            TrafikLabSettings trafikLabSettings,
            IJsonDeserializer jsonDeserializer)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _trafikLabSettings = trafikLabSettings ?? throw new ArgumentNullException(nameof(trafikLabSettings));
            _jsonDeserializer = jsonDeserializer ?? throw new ArgumentNullException(nameof(jsonDeserializer));
            _circuitBreakerPolicy = Policy
                .Handle<Exception>(exception =>
                {
                    if (exception is HttpRequestException || exception is TaskCanceledException)
                    {
                        // TODO log.
                        return true;
                    }

                    return false;
                })
                .CircuitBreakerAsync(3, TimeSpan.FromMinutes(1));
        }

        public async Task<object> GetRealTimeDeparturesV4(int siteId)
        {
            return await SendAsync<object>(CreateRealTimeDeparturesV4Request(siteId));
        }

        public async Task<LineDataV1ResponseDto> GetLineDataV1()
        {
            return await SendAsync<LineDataV1ResponseDto>(CreateLineDataV1Request());
        }

        public async Task<Stream> GetRawLineDataV1()
        {
            return await SendAsync(CreateLineDataV1Request());
        }

        private async Task<T> SendAsync<T>(Func<HttpRequestMessage> requestFactory)
        {
            var policyResult = await _circuitBreakerPolicy.ExecuteAndCaptureAsync(async () =>
            {
                var request = requestFactory.Invoke();
                // TODO error handling
                using (var response = await _httpClient.SendAsync(request))
                using (var responseContentStream = await response.Content.ReadAsStreamAsync())
                {
                    return _jsonDeserializer.Deserialize<T>(responseContentStream);
                }
            });

            if (policyResult.Outcome == OutcomeType.Successful)
            {
                return policyResult.Result;
            }

            throw policyResult.FinalException;
        }

        private async Task<Stream> SendAsync(Func<HttpRequestMessage> requestFactory)
        {
            var policyResult = await _circuitBreakerPolicy.ExecuteAndCaptureAsync(async () =>
            {
                var request = requestFactory.Invoke();
                // TODO error handling
                using (request)
                {
                    // Intentionally not disposing response here since we are going to use the stream later on.
                    var response = await _httpClient.SendAsync(request);
                    return await response.Content.ReadAsStreamAsync();
                }
            });

            if (policyResult.Outcome == OutcomeType.Successful)
            {
                return policyResult.Result;
            }

            throw policyResult.FinalException;
        }

        private Func<HttpRequestMessage> CreateRealTimeDeparturesV4Request(int siteId)
        {
            return () => new HttpRequestMessage(HttpMethod.Get, $"{ApiPath}/realtimedeparturesV4.json?key={_trafikLabSettings.RealTimeDeparturesV4ApiKey}&siteid={siteId}&timewindow=60");
        }

        private Func<HttpRequestMessage> CreateLineDataV1Request()
        {
            return () => new HttpRequestMessage(HttpMethod.Get, $"{ApiPath}/LineData.json?key={_trafikLabSettings.LineDataV1ApiKey}&model=stop");
        }
    }
}
