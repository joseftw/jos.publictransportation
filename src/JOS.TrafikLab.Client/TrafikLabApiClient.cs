using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace JOS.TrafikLab.Client
{
    public class TrafikLabApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly TrafikLabSettings _trafikLabSettings;
        private const string ApiPath = "/api2";

        public TrafikLabApiClient(HttpClient httpClient, TrafikLabSettings trafikLabSettings)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _trafikLabSettings = trafikLabSettings ?? throw new ArgumentNullException(nameof(trafikLabSettings));
        }

        public async Task<object> GetRealTimeDeparturesV4(int siteId)
        {
            var request = CreateRealTimeDeparturesV4Request(siteId);
            var response = await _httpClient.SendAsync(request.Invoke(), HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync();
            return null;
        }

        private Func<HttpRequestMessage> CreateRealTimeDeparturesV4Request(int siteId)
        {
            return () => new HttpRequestMessage(HttpMethod.Get, $"{ApiPath}/realtimedeparturesV4.json?key={_trafikLabSettings.ApiKey}&siteid={siteId}&timewindow=60");
        }
    }
}
