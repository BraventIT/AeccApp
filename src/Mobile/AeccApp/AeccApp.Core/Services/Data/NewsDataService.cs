using Aecc.Models;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public class NewsDataService : BaseDataListsService<NewsModel>, INewsDataService
    {
        protected override string FileName => "News.json";

        public override int MaxItems { get { return 20; } }

        public Task InsertOrUpdateAsync(NewsModel newData)
        {
            return InsertOrIgnoreDataAsync(o => o.NewsId == newData.NewsId, newData);
        }
    }
}
