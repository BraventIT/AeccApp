using Aecc.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IHospitalRequestService
    {
        Task<IList<RequestType>> GetRequestTypesAsync(CancellationToken cancelToken);
        Task<IList<Hospital>> GetHospitalsAsync(string province, CancellationToken cancelToken);
        Task<Hospital> GetHospitalDetail(int hospitalId, CancellationToken cancelToken);
    }
}
