using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IIdentityService
    {
        Task<bool> TryToLoginAsync(bool silentLogin);

        void LogOff();

        Task<string> GetTokenAsync();
        Task EditProfileAsync();
    }
}
