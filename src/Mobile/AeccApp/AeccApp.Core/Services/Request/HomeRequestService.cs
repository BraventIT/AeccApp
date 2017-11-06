using AeccApi.Models;
using AeccApp.Core.Models;
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

        public Task<IEnumerable<RequestType>> GetRequestTypesAsync(CancellationToken cancelToken)
        {
            UriBuilder uribuilder = new UriBuilder(GlobalSetting.Instance.ApiEndpoint)
            {
                Path = "api/RequestTypes",
                Query = $"requestSource={RequestSourceEnum.Domicilio.ToString()}"
            };
            return _requestProvider.GetAsync<IEnumerable<RequestType>>(uribuilder.ToString(), cancelToken);
        }

        public Task<IEnumerable<Coordinator>> GetCoordinatorsAsync(string province, CancellationToken cancelToken)
        {
            UriBuilder uribuilder = new UriBuilder(GlobalSetting.Instance.ApiEndpoint)
            {
                Path = "api/Coordinators",
                Query = $"requestSource={RequestSourceEnum.Hospital.ToString()}&province={province}"
            };
            return _requestProvider.GetAsync<IEnumerable<Coordinator>>(uribuilder.ToString(), cancelToken);
        }
    }
}
