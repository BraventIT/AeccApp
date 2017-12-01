using System;
using System.Threading.Tasks;
using Aecc.Models;
using System.Threading;

namespace AeccApp.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly IHttpRequestProvider _requestProvider;
        private IIdentityService IdentityService { get; } = ServiceLocator.IdentityService;

        public EmailService(IHttpRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task SendAsync(EmailMessage emailMessage, CancellationToken cancelToken)
        {
            UriBuilder uribuilder = new UriBuilder(GlobalSetting.Instance.ApiEndpoint)
            {
                Path = "api/Email"
            };
            var identityToken = await IdentityService.GetTokenAsync();
            await _requestProvider.PostAsync(uribuilder.ToString(), emailMessage, cancelToken, identityToken);
        }
    }
}
