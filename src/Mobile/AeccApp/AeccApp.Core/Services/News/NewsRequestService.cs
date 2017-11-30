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
        private readonly IIdentityService _identityService;

        public NewsRequestService(IIdentityService identityService, IHttpRequestProvider requestProvider)
        {
            _identityService = identityService;
            _requestProvider = requestProvider;
        }

        public async Task<IList<NewsModel>> GetNewsAsync(CancellationToken cancelToken, int? numNewsToLoad  = 2)
        {
            UriBuilder uribuilder = new UriBuilder(GlobalSetting.Instance.ApiEndpoint)
            {
                Path = "api/NewsChannel",
                Query = $"numNewsToLoad={numNewsToLoad}"
            };
            
            var identityToken = await _identityService.GetTokenAsync();
            return await _requestProvider.GetAsync<IList<NewsModel>>(uribuilder.ToString(), cancelToken, identityToken);
        }
    }
}
