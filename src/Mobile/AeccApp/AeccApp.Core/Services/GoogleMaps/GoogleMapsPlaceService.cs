using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AeccApp.Core.Models;

namespace AeccApp.Core.Services
{
    public class GoogleMapsPlaceService : IGoogleMapsPlaceService
    {
        private readonly IHttpRequestProvider _requestProvider;
        private const string GOOGLE_MAPS_ENDPOINT = "https://maps.googleapis.com";

        public GoogleMapsPlaceService(IHttpRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<GooglePlacesDetailModel> GetPlaceDetailAsync(string placeId)
        {
            UriBuilder uriBuilder = new UriBuilder(GOOGLE_MAPS_ENDPOINT)
            {
                Path = "maps/api/place/details/json",
                Query = $"placeid={placeId}&language=es&key={GlobalSetting.Instance.GooglePlacesApiKey}"
            };

            GooglePlacesDetailModel place = await _requestProvider.GetAsync<GooglePlacesDetailModel>(uriBuilder.ToString());
            return place;
        }


        public async Task<IEnumerable<AddressModel>> FindPlacesAsync(string findText)
        {
            UriBuilder uriBuilder = new UriBuilder(GOOGLE_MAPS_ENDPOINT)
            {
                Path = "maps/api/place/autocomplete/json",
                Query = $"input={findText}&language=es&components=country:es&types=geocode&key={GlobalSetting.Instance.GooglePlacesApiKey}"
            };

            var places = await _requestProvider.GetAsync<GooglePlacesModel>(uriBuilder.ToString());

            if (places?.Predictions != null)
                return places.Predictions.Select(item => new AddressModel(item));
            else
                return new List<AddressModel>();
        }
    }
}
