using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace AeccApp.Core.Services.GoogleMaps
{
    class MapPinsDataService : BaseDataService<Pin>, IMapPinsDataService
    {
        private const string MAP_PINS_FILENAME = "MapPins.json";


    public Task<Dictionary<string, Pin>> GetListAsync()
    {
        return LoadDictionaryAsync(MAP_PINS_FILENAME);
    }

    public Task AddOrUpdateAddressAsync(Pin pin)
    {
        return AddOrUpdateDataAsync(MAP_PINS_FILENAME, o => o.Label == pin.Label, pin);
    }
}

}
   
