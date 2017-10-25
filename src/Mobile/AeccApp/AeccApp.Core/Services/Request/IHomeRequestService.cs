using AeccApi.Models;
using AeccApp.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IHomeRequestService
    {
        Task<IEnumerable<RequestType>> GetRequestTypesAsync();
        Task<IEnumerable<Coordinator>> GetCoordinators(string province);
    }
}
