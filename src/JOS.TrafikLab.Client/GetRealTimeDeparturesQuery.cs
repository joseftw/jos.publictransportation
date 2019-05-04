using System;
using System.Threading.Tasks;

namespace JOS.TrafikLab.Client
{
    public class GetRealTimeDeparturesQuery
    {
        private readonly TrafikLabClientFactory _trafikLabClientFactory;

        public GetRealTimeDeparturesQuery(TrafikLabClientFactory trafikLabClientFactory)
        {
            _trafikLabClientFactory = trafikLabClientFactory ?? throw new ArgumentNullException(nameof(trafikLabClientFactory));
        }

        public async Task<object> Execute(int siteId)
        {
            var client = _trafikLabClientFactory.Create();
            var result = await client.GetRealTimeDeparturesV4(siteId);
            return "HEJ";
        }
    }
}
