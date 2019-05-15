using System;

namespace JOS.PublicTransportation.Web.Features.Geolocation
{
    public class PositionViewModel
    {
        public PositionViewModel(
            double longitude,
            double latitude,
            double speed,
            double accuracy,
            DateTime updated)
        {
            Longitude = longitude;
            Latitude = latitude;
            Accuracy = accuracy;
            SpeedInKmPerHour = speed * 3.6;
            Updated = updated.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public double Longitude { get; }
        public double Latitude { get; }
        public double SpeedInKmPerHour { get; }
        public double Accuracy { get; }
        public string Updated { get; }
    }
}
