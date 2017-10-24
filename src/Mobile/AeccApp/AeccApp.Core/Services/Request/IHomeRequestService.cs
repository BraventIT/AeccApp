using AeccApp.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IHomeRequestService
    {
        Task<IEnumerable<RequestTypeModel>> GetRequestTypes();
        Task<IEnumerable<CoordinatorModel>> GetCoordinators(string province);
    }
}
