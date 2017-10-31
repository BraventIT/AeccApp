using AeccApp.Core.Models;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public class AddressesDataService : BaseDataListsService<AddressModel>, IAddressesDataService
    {
        protected override string FileName => "HomeAddresses.json";
        public Task AddOrUpdateAsync(AddressModel address)
        {
            return AddOrUpdateDataAsync(o => o.Name == address.Name, address);
        }
    }
}
