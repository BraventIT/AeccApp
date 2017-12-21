using Aecc.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public class HomeRequestsTypesDataService : BaseDataListsService<RequestType>, IHomeRequestsTypesDataService
    {
        protected override string FileName => "HomeRequestsTypes.json";

        public Task InsertOrUpdateAsync(RequestType requestType)
        {
            return InsertDataAsync(requestType);
        }
    }
}
