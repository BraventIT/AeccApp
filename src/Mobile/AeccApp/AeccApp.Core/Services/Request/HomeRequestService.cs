using AeccApi.Models;
using AeccApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public class HomeRequestService: IHomeRequestService
    {
        private readonly IHttpRequestProvider _requestProvider;
        private readonly IIdentityService _identityService;

        public HomeRequestService(IIdentityService identityService, IHttpRequestProvider requestProvider)
        {
            _identityService = identityService;
            _requestProvider = requestProvider;
        }

        public async Task<IEnumerable<RequestType>> GetRequestTypesAsync()
        {
            UriBuilder uribuilder = new UriBuilder(GlobalSetting.Instance.ApiEndpoint)
            {
                Path = "api/RequestTypes",
                Query = $"requestSource={RequestSourceEnum.Domicilio.ToString()}"
            };
            return await _requestProvider.GetAsync<IEnumerable<RequestType>>(uribuilder.ToString());
        }

        public async Task<IEnumerable<Coordinator>> GetCoordinatorsAsync(string province)
        {
            UriBuilder uribuilder = new UriBuilder(GlobalSetting.Instance.ApiEndpoint)
            {
                Path = "api/Coordinators",
                Query = $"requestSource={RequestSourceEnum.Hospital.ToString()}&province={province}"
            };
            return await _requestProvider.GetAsync<IEnumerable<Coordinator>>(uribuilder.ToString());
        }
    }
}
