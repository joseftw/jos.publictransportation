using System;

namespace JOS.PublicTransportation.Web.Features.Geolocation
{
    public class GeolocationDebugComponentViewModel
    {
        public GeolocationDebugComponentViewModel()
        {
            Position = new PositionViewModel(0,0,0,0, DateTime.UtcNow);
        }
        public PositionViewModel Position { get; set; }
    }
}
