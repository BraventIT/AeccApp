using Plugin.Geolocator;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using Plugin.Geolocator.Abstractions;

namespace AeccApp.Core.Extensions
{
    public static class GeolocatorExtensions
    {
        public static bool IsLocationAvailable(this IGeolocator geolocator)
        {
            if (!CrossGeolocator.IsSupported)
                return false;

            return geolocator.IsGeolocationAvailable;
        }

        public static async Task<Xamarin.Forms.GoogleMaps.Position> GetCurrentLocationAsync(this IGeolocator locator, CancellationToken cancelToken)
        {
            Plugin.Geolocator.Abstractions.Position position = null;
            try
            {
                locator.DesiredAccuracy = 100;

                //got a cahched position, so let's use it.
                position = await locator.GetLastKnownLocationAsync();

                if (position == null)
                    position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), cancelToken, true);
            }
            catch (Exception ex)
            {
                //Display error as we have timed out or can't get location.
                Debug.WriteLine(ex);
            }

#if DEBUG
            if (position != null)
            {

                var output = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                    position.Timestamp, position.Latitude, position.Longitude,
                    position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

                Debug.WriteLine(output);
            }
#endif
            return (position != null) ?
                new Xamarin.Forms.GoogleMaps.Position(position.Latitude, position.Longitude) : new Xamarin.Forms.GoogleMaps.Position();
        }
    }
}
