using System;
using System.Collections.Generic;
using System.Text;

namespace AeccApp.Core.Messages
{

    public enum GeolocatorEnum
    {
        Refresh = 0
    }
    class GeolocatorMessages
    {


        public GeolocatorEnum Message { get; set; }

        public GeolocatorMessages(GeolocatorEnum tab)
        {
            Message = tab;
        }

    }
}
