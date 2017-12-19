using Aecc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface INewsDataService
    {
        int MaxItems { get; }
        int Count { get; }

        Task<List<NewsModel>> GetListAsync();

        Task InsertOrUpdateAsync(NewsModel newData);

        Task ResetAsync();
    }
}
