using AeccApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IHospitalRequestService
    {
        Task<IEnumerable<RequestType>> GetRequestTypesAsync();
        Task<IEnumerable<Hospital>> GetHospitalsAsync(string province);
        Task<Hospital> GetHospitalDetail(int hospitalId);
    }
}
