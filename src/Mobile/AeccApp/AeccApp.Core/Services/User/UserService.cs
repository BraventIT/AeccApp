using System;
using System.Threading.Tasks;
using AeccApp.Core.Models;

namespace AeccApp.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly IIdentityService _identityService;

        public UserService(IIdentityService identityService, IRequestProvider requestProvider)
        {
            _identityService = identityService;
            _requestProvider = requestProvider;
        }


        public async Task<UserData> GetUserAsync()
        {
            UriBuilder uribuilder = new UriBuilder(GlobalSetting.Instance.BaseEndpoint);
            uribuilder.Path += "users/info";
            
            var token = await _identityService.GetTokenAsync();
            return await _requestProvider.GetAsync<UserData>(uribuilder.ToString(), token);
        }
    }
}
