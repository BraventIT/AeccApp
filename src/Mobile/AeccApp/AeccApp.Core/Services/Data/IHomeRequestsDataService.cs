using AeccApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
   public interface IHomeRequestsDataService
    {
        Task<List<RequestModel>> GetListAsync();

        Task InsertOrUpdateAsync(RequestModel request);
    }
}
