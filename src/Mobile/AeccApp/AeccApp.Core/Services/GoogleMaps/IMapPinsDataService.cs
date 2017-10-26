using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace AeccApp.Core.Services.GoogleMaps
{
    public interface IMapPinsDataService
    {
        Task<Dictionary<string,Pin>> GetListAsync();

        Task AddOrUpdateAddressAsync(Pin pin);
    }
}
