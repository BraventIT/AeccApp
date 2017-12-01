using Aecc.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public class NewsRequestService : INewsRequestService
    {
        private readonly IHttpRequestProvider _requestProvider;
        private IIdentityService IdentityService { get; } = ServiceLocator.IdentityService;

        public NewsRequestService(IHttpRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<IList<NewsModel>> GetNewsAsync(CancellationToken cancelToken, int? numNewsToLoad  = 2)
        {
            UriBuilder uribuilder = new UriBuilder(GlobalSetting.Instance.ApiEndpoint)
            {
                Path = "api/NewsChannel",
                Query = $"numNewsToLoad={numNewsToLoad}"
            };
            
            var identityToken = await IdentityService.GetTokenAsync();
            return await _requestProvider.GetAsync<IList<NewsModel>>(uribuilder.ToString(), cancelToken, identityToken);
        }
    }
}
