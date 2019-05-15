window.josGeolocation = {
    serialize: function (e) {
        return {
            "coords": {
                "latitude": e.coords.latitude,
                "longitude": e.coords.longitude,
                "accuracy": e.coords.accuracy,
                "altitude": e.coords.altitude,
                "altitudeAccuracy": e.coords.altitudeAccuracy,
                "heading": e.coords.heading,
                "speed": e.coords.speed
            },
            "timestamp": new Date(e.timestamp)
        };
    },
    hasGeolocationFeature: function () {
        return navigator.geolocation ? true : false;
    },
    getCurrentPosition: function (geolocationRef, options) {
        const success = (result) => {
            geolocationRef.invokeMethodAsync('RaiseOnGetPosition', josGeolocation.serialize(result));
        };
        const error = (er) =>
            geolocationRef.invokeMethodAsync('RaiseOnGetPositionError', er.code);
        if (josGeolocation.hasGeolocationFeature()) {
            navigator.geolocation.getCurrentPosition(success, error, options);
        }
    },
    watchPosition: function (geolocationRef, options) {
        const success = (result) =>
            geolocationRef.invokeMethodAsync('RaiseOnWatchPosition', josGeolocation.serialize(result));
        const error = (er) =>
            geolocationRef.invokeMethodAsync('RaiseOnWatchPositionError', er.code);
        if (josGeolocation.hasGeolocationFeature()) {
            return navigator.geolocation.watchPosition(success, error, options);
        }
        return 0;
    },
    clearWatch: function (watchId) {
        if (josGeolocation.hasGeolocationFeature()) {
            navigator.geolocation.clearWatch(watchId);
        }
    }
};