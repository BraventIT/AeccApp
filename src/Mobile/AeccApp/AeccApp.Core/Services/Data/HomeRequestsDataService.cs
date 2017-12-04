using AeccApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public class HomeRequestsDataService : BaseDataListsService<RequestModel>, IHomeRequestsDataService
    {
        protected override string FileName => "HomeRequests.json";

        public override int MaxItems { get { return 10; } }

        public Task InsertOrUpdateAsync(RequestModel request)
        {
            return InsertDataAsync(request);
        }
    }
}
