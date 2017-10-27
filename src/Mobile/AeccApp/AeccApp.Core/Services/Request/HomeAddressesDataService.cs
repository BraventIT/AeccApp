using AeccApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace AeccApp.Core.Services
{
    public class HomeAddressesDataService : BaseDataListsService<AddressModel>, IHomeAddressesDataService
    {
        public override string FileName => "HomeAddresses.json";
        public Task AddOrUpdateAddressAsync(AddressModel address)
        {
            return AddOrUpdateDataAsync(o => o.Name == address.Name, address);
        }
    }
}
