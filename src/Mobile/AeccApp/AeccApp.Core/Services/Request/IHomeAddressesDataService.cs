using AeccApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IHomeAddressesDataService
    {
        Task<List<AddressModel>> GetListAsync();

        Task AddOrUpdateAddressAsync(AddressModel address);
    }
}
