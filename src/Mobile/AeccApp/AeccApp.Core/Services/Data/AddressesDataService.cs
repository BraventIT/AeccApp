using AeccApp.Core.Models;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public class AddressesDataService : BaseDataListsService<AddressModel>, IAddressesDataService
    {
        protected override string FileName => "HomeAddresses.json";

        public override int MaxItems { get { return 10; } }

        public Task InsertOrUpdateAsync(AddressModel address)
        {
            return InsertOrUpdateDataAsync(o => o.Name == address.Name, address);
        }
    }
}
