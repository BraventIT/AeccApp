using System.Linq;
using Xamarin.Forms.GoogleMaps;

namespace AeccApp.Core.Models
{
    public class AddressModel
    {
        public bool IsHospitalAddress { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Number { get; set; }
        public string Floor { get; set; }
        public string Portal { get; set; }
        public string HospitalRoom { get; set; }
        public string HospitalHall { get; set; }
        public bool WillBeSaved { get; set; }
        public string PlaceId { get; set; }
        public int HospitalID { get; set; }
        private string _displayRoom;

        public string DisplayRoom
        {
            get
            {
                if (HospitalRoom == null || HospitalRoom == string.Empty)
                {
                    return "sin especificar";
                }
                else
                {
                    return HospitalRoom;
                }
            }

            set { _displayRoom = value; }
        }

      


        private Position _coordinates;

        public Position Coordinates
        {
            get { return _coordinates; }
            set { _coordinates = value; }
        }

        public string FinderAddress
        {
            get
            {
                return string.IsNullOrEmpty(Number) ?
                  $"{Street}, {City}" :
                  $"{Street} {Number}, {City}";
            }
        }

        public string DisplayAddress
        {
            get
            {
                var address = FinderAddress;
                return string.IsNullOrEmpty(Name) ?
                      address :
                       $"{Name} - {address}";
            }
        }

        public AddressModel(Prediction item)
        {
            PlaceId = item.PlaceId;
            int numTerms = item.Terms.Count();

            if (numTerms < 2)
            {
                return;
            }

            switch (numTerms)
            {
                case 3:
                    Street = item.Terms[0].Value;
                    break;
                case 4:
                case 5:
                    Street = item.Terms[0].Value;
                    if (int.TryParse(item.Terms[1].Value, out int number))
                    {
                        Number = item.Terms[1].Value;
                    }
                    break;
                default:
                    break;
            }

            City = item.Terms[numTerms - 2].Value;
        }

        public AddressModel()
        {

        }

        public AddressModel(string name, string street, string province, string number, string floor, string placeId, Position Coordinates)
        {
            Name = name;
            Street = street;
            Province = province;
            Number = number;
            Floor = floor;
            PlaceId = placeId;
            this.Coordinates = Coordinates;
        }


    }
}
