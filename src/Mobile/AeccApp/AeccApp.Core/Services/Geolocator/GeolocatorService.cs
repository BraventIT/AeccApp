using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Geolocator.Abstractions;
using Plugin.Geolocator;

namespace AeccApp.Core.Services.Geolocator
{
    public class GeolocatorService : IGeolocatorService
    {
        public IGeolocator GetCurrent()
        {
            return CrossGeolocator.Current;
        }

        public bool GetIsSupported()
        {
            return CrossGeolocator.IsSupported;
        }
    }
}
