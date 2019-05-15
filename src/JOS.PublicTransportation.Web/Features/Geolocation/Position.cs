using System;

namespace JOS.PublicTransportation.Web.Features.Geolocation
{
    public class Position
    {
        public Coords Coords { get; set; }
        public DateTime Timestamp { get; set; }
    }
}