﻿using AeccApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public class HospitalRequestService : IHospitalRequestService
    {
        private readonly IHttpRequestProvider _requestProvider;
        private readonly IIdentityService _identityService;

        public HospitalRequestService(IIdentityService identityService, IHttpRequestProvider requestProvider)
        {
            _identityService = identityService;
            _requestProvider = requestProvider;
        }

        public async Task<IEnumerable<RequestType>> GetRequestTypesAsync()
        {
            UriBuilder uribuilder = new UriBuilder(GlobalSetting.Instance.ApiEndpoint)
            {
                Path = "api/RequestTypes",
                Query = $"requestSource={RequestSourceEnum.Hospital.ToString()}"
            };
            return await _requestProvider.GetAsync<IEnumerable<RequestType>>(uribuilder.ToString());
        }

        public async Task<IEnumerable<Hospital>> GetHospitalsAsync(string province)
        {
            UriBuilder uribuilder = new UriBuilder(GlobalSetting.Instance.ApiEndpoint)
            {
                Path = "api/Hospitals",
                Query = $"province={province}"
            };
            return await _requestProvider.GetAsync<IEnumerable<Hospital>>(uribuilder.ToString());
        }

        public async Task<Hospital> GetHospitalDetail(int hospitalId)
        {
            UriBuilder uribuilder = new UriBuilder(GlobalSetting.Instance.ApiEndpoint)
            {
                Path = $"api/Hospitals/{hospitalId}"
            };
            return await _requestProvider.GetAsync<Hospital>(uribuilder.ToString());
        }
    }
}