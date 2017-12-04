using AeccApp.Core.Models;
using AeccApp.Core.Services;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
   public class HospitalRequestDataService : BaseDataListsService<RequestModel>, IHospitalRequestDataService
    {
        protected override string FileName => "HospitalRequests.json";

        public override int MaxItems { get { return 10; } }

        public Task InsertOrUpdateAsync(RequestModel request)
        {
            return InsertDataAsync(request);
        }
    }
}