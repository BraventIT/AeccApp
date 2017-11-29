using AeccApp.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace AeccApp.Core.Services
{
    public interface IMapPositionsDataService
    {
        Task<Position> GetAsync(string key);

        Task InsertOrUpdateAsync(string key ,Position pos);
    }
}
