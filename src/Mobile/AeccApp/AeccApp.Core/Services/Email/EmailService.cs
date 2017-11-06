using System;
using System.Threading.Tasks;
using AeccApi.Models;
using System.Threading;

namespace AeccApp.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly IHttpRequestProvider _requestProvider;
        private readonly IIdentityService _identityService;

        public EmailService(IIdentityService identityService, IHttpRequestProvider requestProvider)
        {
            _identityService = identityService;
            _requestProvider = requestProvider;
        }

        public Task SendAsync(EmailMessage emailMessage, CancellationToken cancelToken)
        {
            UriBuilder uribuilder = new UriBuilder(GlobalSetting.Instance.ApiEndpoint)
            {
                Path = "api/Email"
            };
            return _requestProvider.PostAsync(uribuilder.ToString(), emailMessage, cancelToken);
        }
    }
}
