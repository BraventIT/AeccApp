using AeccApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public class HomeAddressesDataService: BaseDataService<AddressModel>, IHomeAddressesDataService
    {
        private const string HOMEADDRESSES_FILENAME = "HomeAddresses.json";

     
        public Task<List<AddressModel>> GetListAsync()
        {
            return LoadAsync(HOMEADDRESSES_FILENAME);
        }

        public Task AddOrUpdateAddressAsync(AddressModel address)
        {
            return AddOrUpdateDataAsync(HOMEADDRESSES_FILENAME, o => o.Name == address.Name, address);
        }
    }
}
