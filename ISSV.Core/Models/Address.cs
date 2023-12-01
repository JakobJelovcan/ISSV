using System.ComponentModel.DataAnnotations;

namespace ISSV.Core.Models
{
    public class Address
    {
        public Address() { }

        public Address(string name, double latitude, double longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }

        [Key]
        public string Name { get; private set; }

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }
    }
}
