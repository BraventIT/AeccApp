using System;
using System.Threading.Tasks;
using AeccApp.Core.Models;

namespace AeccApp.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpRequestProvider _requestProvider;
        private IIdentityService IdentityService { get; } = ServiceLocator.IdentityService;

        public UserService( IHttpRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }


        public async Task<UserData> GetUserAsync()
        {
            UriBuilder uribuilder = new UriBuilder(GlobalSetting.Instance.ApiEndpoint);
            uribuilder.Path += "users/info";
            
            var token = await IdentityService.GetTokenAsync();
            return await _requestProvider.GetAsync<UserData>(uribuilder.ToString(), token: token);
        }
    }
}
