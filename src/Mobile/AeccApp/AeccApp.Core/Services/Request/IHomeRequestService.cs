using Aecc.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IHomeRequestService
    {
        Task<IList<RequestType>> GetRequestTypesAsync(CancellationToken cancelToken);
        Task<IList<Coordinator>> GetCoordinatorsAsync(string province, CancellationToken cancelToken);
    }
}
