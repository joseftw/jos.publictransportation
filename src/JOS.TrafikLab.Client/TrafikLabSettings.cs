namespace JOS.TrafikLab.Client
{
    public class TrafikLabSettings
    {
        public string ApiKey { get; }

        public TrafikLabSettings(string apiKey)
        {
            ApiKey = apiKey;
        }
    }
}
