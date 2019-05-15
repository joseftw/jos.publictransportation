namespace JOS.PublicTransportation.Web.Features.Geolocation
{
    public class PositionOptions
    {
        public bool EnableHighAccuracy { get; set; }

        public int Timeout { get; set; }

        public int MaximumAge { get; set; } = 0;
    }
}