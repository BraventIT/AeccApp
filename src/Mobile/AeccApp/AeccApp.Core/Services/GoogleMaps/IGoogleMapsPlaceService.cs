using AeccApp.Core.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IGoogleMapsPlaceService
    {
        Task<IEnumerable<AddressModel>> FindPlacesAsync(string findText);
    }
}
