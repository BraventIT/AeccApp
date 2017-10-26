using AeccApp.Core.Services;
using AeccApp.Core.Services.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace AeccApp.Core.Services
{
    public class MapPinsDataService : BaseDataDictionaryService<Pin>, IMapPinsDataService
    {
        private const string MAP_PINS_FILENAME = "MapPins.json";

        public override string FileName => throw new System.NotImplementedException();

        public Task<Dictionary<string, Pin>> GetListAsync()
    {
        return LoadAsync();
    }

    public Task AddOrUpdateAddressAsync(string key , Pin pin)
    {
        return AddOrUpdateDataAsync(key, pin);
    }
}

}
   
