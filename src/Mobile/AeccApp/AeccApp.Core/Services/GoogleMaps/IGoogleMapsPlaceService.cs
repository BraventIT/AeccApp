using AeccApp.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IGoogleMapsPlaceService
    {
        Task<IEnumerable<AddressModel>> FindPlacesAsync(string findText);

        Task<AddressModel> FillPlaceDetailAsync(AddressModel address);

    }
}
