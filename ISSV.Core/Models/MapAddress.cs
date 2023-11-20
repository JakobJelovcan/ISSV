using System;
using System.Collections.Generic;
using System.Text;

namespace ISSV.Core.Models
{
    public class MapAddress
    {
        public MapAddress(string address, double latitude, double longitude)
        {
            this.Address = address;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
        
        public string Address { get; private set; }

        public double Latitude { get; private set; }

        public double Longitude { get; private set;}
    }
}
