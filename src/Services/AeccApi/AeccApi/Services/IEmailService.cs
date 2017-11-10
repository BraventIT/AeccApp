using System.Threading.Tasks;
using AeccApi.Models;
using Aecc.Models;

namespace AeccApi.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailOptions emailData, EmailMessage emailMessage);
    }
}
