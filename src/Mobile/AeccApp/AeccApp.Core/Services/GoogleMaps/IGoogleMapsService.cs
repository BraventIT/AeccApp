using AeccApp.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IGoogleMapsService
    {
        Task<IEnumerable<AddressModel>> FindPlacesAsync(string findText, Position position);

        Task<AddressModel> FillPlaceDetailAsync(AddressModel address);

        Task<Position> FindAddressGeocodingAsync(string address);

        Task<AddressModel> FindCoordinatesGeocodingAsync(double lat, double lng);
    }
}
