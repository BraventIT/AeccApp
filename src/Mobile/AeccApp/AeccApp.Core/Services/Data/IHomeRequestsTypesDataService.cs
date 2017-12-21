using Aecc.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IHomeRequestsTypesDataService
    {
        Task<List<RequestType>> GetListAsync();

        Task InsertOrUpdateAsync(RequestType requestType);

        Task ResetAsync();
    }
}
