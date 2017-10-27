using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace AeccApp.Core.Services
{
    public interface IMapPinsDataService
    {
        Task<Dictionary<string,Pin>> GetListAsync();

        Task AddOrUpdateAddressAsync(string key ,Pin pin);
    }
}
