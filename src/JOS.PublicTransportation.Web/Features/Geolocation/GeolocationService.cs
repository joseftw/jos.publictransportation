using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace JOS.PublicTransportation.Web.Features.Geolocation
{
    public class GeolocationService
    {
        private const string JsPrefix = "josGeolocation";
        private readonly IJSRuntime _jsRuntime;

        public GeolocationService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        }

        private Action<Position> _onWatchPosition;
        private Action<Position> _onGetPosition;
        private Action<PositionError> _onWatchPositionError;
        private Action<PositionError> _onGetPositionError;

        public async Task GetCurrentPosition(
            Action<Position> onSuccess,
            Action<PositionError> onError,
            PositionOptions options)
        {
            _onGetPosition = onSuccess;
            _onGetPositionError = onError;
            await _jsRuntime.InvokeAsync<bool>($"{JsPrefix}.getCurrentPosition", new DotNetObjectRef(this), options);
        }

        public async Task<bool> HasGeolocationFeature()
        {
            return await _jsRuntime.InvokeAsync<bool>($"{JsPrefix}.hasGeolocationFeature");
        }

        public async Task<int> WatchPosition(
            Action<Position> onSuccess,
            Action<PositionError> onError,
            PositionOptions options)
        {
            _onWatchPosition = onSuccess;
            _onWatchPositionError = onError;
            return await _jsRuntime.InvokeAsync<int>($"{JsPrefix}.watchPosition", new DotNetObjectRef(this), options);
        }

        public async Task ClearWatch(int watchId)
        {
            await _jsRuntime.InvokeAsync<int>($"{JsPrefix}.clearWatch", watchId);
        }

        [JSInvokable]
        public void RaiseOnGetPosition(Position error)
        {
            _onGetPosition?.Invoke(error);
        }

        [JSInvokable]
        public void RaiseOnGetPositionError(PositionError err)
        {
            _onGetPositionError?.Invoke(err);
        }

        [JSInvokable]
        public void RaiseOnWatchPosition(Position position)
        {
            _onWatchPosition?.Invoke(position);
        }

        [JSInvokable]
        public void RaiseOnWatchPositionError(PositionError error)
        {
            _onWatchPositionError?.Invoke(error);
        }
    }
}
