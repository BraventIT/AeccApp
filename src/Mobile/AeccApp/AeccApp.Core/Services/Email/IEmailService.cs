using AeccApi.Models;
using AeccApp.Core.Models.Email;
using System.Threading;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailMessage emailMessage, CancellationToken cancelToken);
    }
}
