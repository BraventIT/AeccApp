using Aecc.Models;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public class HomeRequestsTypesDataService : BaseDataListsService<RequestType>, IHomeRequestsTypesDataService
    {
        protected override string FileName => "HomeRequestsTypes.json";

        public Task InsertOrUpdateAsync(RequestType newData)
        {
            return InsertOrUpdateDataAsync(o => o.Id == newData.Id, newData);
        }
    }
}
