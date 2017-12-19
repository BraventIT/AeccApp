using AeccApp.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IAddressesDataService
    {
        Task<List<AddressModel>> GetListAsync();

        Task InsertOrUpdateAsync(AddressModel address);

        Task ResetAsync();
    }
}
