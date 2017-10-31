using AeccApp.Core.Models;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public class MapPositionsDataService : BaseDataDictionaryService<Position>, IMapPositionsDataService
    {
        public override string FileName => "MapPositions.json";

        public Task AddOrUpdateAsync(string key, Position pos)
        {
            return AddOrUpdateDataAsync(key, pos);
        }
    }
}
   
