using AeccApp.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IMapPositionsDataService
    {
        Task<Position> GetAsync(string key);

        Task AddOrUpdateAsync(string key ,Position pos);
    }
}
