using Aecc.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public class HomeRequestService : IHomeRequestService
    {
        private readonly IHttpRequestProvider _requestProvider;
        private readonly IIdentityService _identityService;

        public HomeRequestService(IIdentityService identityService, IHttpRequestProvider requestProvider)
        {
            _identityService = identityService;
            _requestProvider = requestProvider;
        }

        public async Task<IEnumerable<RequestType>> GetRequestTypesAsync(CancellationToken cancelToken)
        {
            UriBuilder uribuilder = new UriBuilder(GlobalSetting.Instance.ApiEndpoint)
            {
                Path = "api/RequestTypes",
                Query = $"requestSource={RequestSourceEnum.Domicilio.ToString()}"
            };
            var identityToken = await _identityService.GetTokenAsync();
            return await _requestProvider.GetAsync<IEnumerable<RequestType>>(uribuilder.ToString(), cancelToken, identityToken);
        }

        public async Task<IEnumerable<Coordinator>> GetCoordinatorsAsync(string province, CancellationToken cancelToken)
        {
            UriBuilder uribuilder = new UriBuilder(GlobalSetting.Instance.ApiEndpoint)
            {
                Path = "api/Coordinators",
                Query = $"requestSource={RequestSourceEnum.Hospital.ToString()}&province={province}"
            };
            var identityToken = await _identityService.GetTokenAsync();
            return await _requestProvider.GetAsync<IEnumerable<Coordinator>>(uribuilder.ToString(), cancelToken, identityToken);
        }
    }
}
