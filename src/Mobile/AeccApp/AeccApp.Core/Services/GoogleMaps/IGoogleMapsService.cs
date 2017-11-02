using AeccApp.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace AeccApp.Core.Services
{
    public interface IGoogleMapsService
    {
        Task<IEnumerable<AddressModel>> FindPlacesAsync(string findText, Position position, CancellationToken cancelToken);

        Task<AddressModel> FillPlaceDetailAsync(AddressModel address, CancellationToken cancelToken);

        Task<Position> FindAddressGeocodingAsync(string address, CancellationToken cancelToken);

        Task<AddressModel> FindCoordinatesGeocodingAsync(double lat, double lng, CancellationToken cancelToken);
    }
}
