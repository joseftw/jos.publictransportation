using System;

namespace JOS.TrafikLab.Client
{
    public class TrafikLabSettings
    {
        public string RealTimeDeparturesV4ApiKey { get; }
        public string LineDataV1ApiKey { get; }
        public int DefaultTimeoutMs { get; }

        public TrafikLabSettings(string realTimeDeparturesV4ApiKey, string lineDataV1ApiKey, int defaultTimeoutMs)
        {
            RealTimeDeparturesV4ApiKey = realTimeDeparturesV4ApiKey ?? throw new ArgumentNullException(nameof(realTimeDeparturesV4ApiKey));
            LineDataV1ApiKey = lineDataV1ApiKey ?? throw new ArgumentNullException(nameof(lineDataV1ApiKey));
            DefaultTimeoutMs = defaultTimeoutMs;
        }
    }
}
