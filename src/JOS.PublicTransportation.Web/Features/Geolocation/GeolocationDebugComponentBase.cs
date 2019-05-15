using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace JOS.PublicTransportation.Web.Features.Geolocation
{
    public class GeolocationDebugComponentBase : ComponentBase, IDisposable
    {
        [Inject]
        protected GeolocationService GeolocationService { get; set; }
        protected bool HasGeolocation;
        protected bool HasGeolocationErrors;
        protected string ErrorMessage;
        protected GeolocationDebugComponentViewModel ViewModel;
        private int _watchId;

        public GeolocationDebugComponentBase()
        {
            ViewModel = new GeolocationDebugComponentViewModel();
        }

        protected override async Task OnInitAsync()
        {
            HasGeolocation = await GeolocationService.HasGeolocationFeature();
            if (HasGeolocation)
            {
                _watchId = await GeolocationService.WatchPosition(OnSuccess, OnError, new PositionOptions
                {
                    EnableHighAccuracy = true,
                    MaximumAge = 0,
                    Timeout = 20000
                });
            }
        }

        private void OnSuccess(Position position)
        {
            ViewModel.Position = new PositionViewModel(
                position.Coords.Longitude,
                position.Coords.Latitude,
                position.Coords.Speed,
                position.Coords.Accuracy,
                position.Timestamp);
            HasGeolocationErrors = false;
            StateHasChanged();
        }

        private void OnError(PositionError positionError)
        {
            HasGeolocationErrors = true;
            switch (positionError)
            {
                case PositionError.PermissionDenied:
                    ErrorMessage = "Permission to access location was denied by the user.";
                    break;
                case PositionError.PositionUnavailable:
                    ErrorMessage = "No location data was available.";
                    break;
                case PositionError.Timeout:
                    ErrorMessage = "Timeout receiving location data.";
                    break;
                default:
                    ErrorMessage = "UNKNOWN ERROR";
                    break;

            }
            StateHasChanged();
        }

        public void Dispose()
        {
            GeolocationService.ClearWatch(_watchId).RunSynchronously();
        }
    }
}
