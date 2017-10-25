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
                Query = "requestSource={RequestSourceEnum.Hospital.ToString()}"
            };
            return await _requestProvider.GetAsync<IEnumerable<RequestType>>(uribuilder.ToString());
        }

        public async Task<IEnumerable<Coordinator>> GetCoordinators(string province)
        {
            await Task.Delay(2000);
            if (province.StartsWith("Barcelona", StringComparison.CurrentCultureIgnoreCase))
            {
                return new Coordinator[]
                {
                    new Coordinator()
                    {
                        Name= "Jordi Abad"
                        , Email="jordi.abad@test.org"
                        , Province="Barcelona"
                    },
                    new Coordinator()
                    {
                        Name="Ana Carretero"
                        , Email="anamaria.carretero@test.org"
                        , Province="Barcelona"
                    },
                };
            }
            else if (province.StartsWith("Madrid", StringComparison.CurrentCultureIgnoreCase))
            {
                return new Coordinator[]
               {
                    new Coordinator()
                    {
                        Name="Eva Sanchez",
                        Email= "eva.sanchez@test.org"
                    }
               };
            }
            else if (province.StartsWith("Gerona", StringComparison.CurrentCultureIgnoreCase))
            {
                return new Coordinator[]
               {
                    new Coordinator()
                    {
                        Name="Alberto Pulez",
                        Email= "alberto.pulez@test.org"
                    }
               };
            }
            else if (province.StartsWith("La Coruña", StringComparison.CurrentCultureIgnoreCase))
            {
                return new Coordinator[]
               {
                    new Coordinator()
                    {
                        Name="Victor Pulez",
                        Email= "Victor.pulez@test.org"
                    }
               };
            }
            else
                return new List<Coordinator>();
        }
    }
}
