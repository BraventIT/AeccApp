using AeccApi.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IHospitalRequestService
    {
        Task<IEnumerable<RequestType>> GetRequestTypesAsync(CancellationToken cancelToken);
        Task<IEnumerable<Hospital>> GetHospitalsAsync(string province, CancellationToken cancelToken);
        Task<Hospital> GetHospitalDetail(int hospitalId, CancellationToken cancelToken);
    }
}
