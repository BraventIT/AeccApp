using Aecc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface INewsDataService
    {
        Task<List<NewsModel>> GetListAsync();

        Task InsertOrUpdateAsync(NewsModel address);
    }
}
