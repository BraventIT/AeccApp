using AeccApi.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IHomeRequestService
    {
        Task<IEnumerable<RequestType>> GetRequestTypesAsync(CancellationToken cancelToken);
        Task<IEnumerable<Coordinator>> GetCoordinatorsAsync(string province, CancellationToken cancelToken);
    }
}
