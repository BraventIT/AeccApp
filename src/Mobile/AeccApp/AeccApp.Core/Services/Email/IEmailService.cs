using Aecc.Models;
using System.Threading;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailMessage emailMessage, CancellationToken cancelToken);
    }
}
