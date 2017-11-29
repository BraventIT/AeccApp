using AeccApp.Core.Models;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace AeccApp.Core.Services
{
    public class MapPositionsDataService : BaseDataDictionaryService<Position>, IMapPositionsDataService
    {
        protected override string FileName => "MapPositions.json";

        public Task InsertOrUpdateAsync(string key, Position pos)
        {
            return AddOrUpdateDataAsync(key, pos);
        }
    }
}
   
