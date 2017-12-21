using Aecc.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public class HospitalRequestsTypesDataService : BaseDataListsService<RequestType>, IHospitalRequestsTypesDataService
    {
        protected override string FileName => "HospitalRequestsTypes.json";

        public Task InsertOrUpdateAsync(RequestType requestType)
        {
            return InsertDataAsync(requestType);
        }
    }
}
