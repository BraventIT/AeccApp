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

        public async Task<IEnumerable<AddressModel>> FindPlacesAsync(string findText, Xamarin.Forms.GoogleMaps.Position position)
        {
            string query = $"input={findText}&language=es&components=country:es&types=geocode&key={GlobalSetting.Instance.GooglePlacesApiKey}";
            if (position.Latitude != 0)
            {
                query += $"&location={position.Latitude},{position.Longitude}";
            }

            UriBuilder uriBuilder = new UriBuilder(GOOGLE_MAPS_ENDPOINT)
            {
                Path = "maps/api/place/autocomplete/json",
                Query = query
            };

            var places = await _requestProvider.GetAsync<GooglePlacesModel>(uriBuilder.ToString());

            if (places?.Predictions != null)
                return places.Predictions.Select(item => new AddressModel(item));
            else
                return new List<AddressModel>();
        }

        public async Task<AddressModel> FillPlaceDetailAsync(AddressModel address)
        {
            UriBuilder uriBuilder = new UriBuilder(GOOGLE_MAPS_ENDPOINT)
            {
                Path = "maps/api/place/details/json",
                Query = $"placeid={address.PlaceId}&language=es&key={GlobalSetting.Instance.GooglePlacesApiKey}"
            };

            GooglePlacesDetailModel place = await _requestProvider.GetAsync<GooglePlacesDetailModel>(uriBuilder.ToString());
            address.AddCoordinates(place);
            return address;
        }
    }
}
