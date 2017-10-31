using AeccApp.Core.Models;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public class HomeAddressesDataService : BaseDataListsService<AddressModel>, IAddressesDataService
    {
        public override string FileName => "HomeAddresses.json";
        public Task AddOrUpdateAsync(AddressModel address)
        {
            return AddOrUpdateDataAsync(o => o.Name == address.Name, address);
        }
    }
}
