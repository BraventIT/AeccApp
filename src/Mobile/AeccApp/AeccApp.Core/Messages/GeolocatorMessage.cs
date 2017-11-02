using System;
using System.Collections.Generic;
using System.Text;

namespace AeccApp.Core.Messages
{

    public enum GeolocatorEnum
    {
        Refresh = 0
    }
    class GeolocatorMessage
    {


        public GeolocatorEnum Message { get; set; }

        public GeolocatorMessage(GeolocatorEnum tab)
        {
            Message = tab;
        }

    }
}
