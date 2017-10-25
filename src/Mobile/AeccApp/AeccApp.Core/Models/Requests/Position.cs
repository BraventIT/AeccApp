using System;
using System.Collections.Generic;
using System.Text;

namespace AeccApp.Core.Models
{
    public class Position
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Position(double lat , double lng)
        {
            this.Latitude = lat;
            this.Longitude = lng;
        }
    }
}
