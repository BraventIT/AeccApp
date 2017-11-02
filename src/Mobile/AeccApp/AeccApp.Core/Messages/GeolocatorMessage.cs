using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace AeccApp.Core.Messages
{

   public  class GeolocatorMessage
    {
        public Position Position { get; set; }

        public GeolocatorMessage(Position position)
        {
            Position = position;
        }
    }
}
