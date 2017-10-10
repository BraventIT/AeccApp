using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AeccApp.Core.Models.Requests;
using System.Linq;

namespace AeccApp.Core.Services
{
    public class GoogleMapsPlaceService : IGoogleMapsPlaceService
    {
        private readonly IRequestProvider _requestProvider;
        private const string GOOGLE_MAPS_ENDPOINT = "https://maps.googleapis.com";

        public GoogleMapsPlaceService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<IEnumerable<AddressModel>> FindPlacesAsync(string findText)
        {
            UriBuilder uriBuilder = new UriBuilder(GOOGLE_MAPS_ENDPOINT)
            {
                Path = "maps/api/place/autocomplete/json",
                Query= $"input={findText}&language=es&types=geocode&key={GlobalSetting.Instance.GooglePlacesApiKey}"
            };

            GooglePlacesModel places = await _requestProvider.GetAsync<GooglePlacesModel>(uriBuilder.ToString());

            if (places?.Predictions != null)
                return places.Predictions.Select(item =>
                new AddressModel()
                {
                    DisplayAddress = item.Description,
                    PlaceId = item.PlaceId
                });
            else
                return new List<AddressModel>();
        }
    }
}
