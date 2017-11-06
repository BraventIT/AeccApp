using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AeccApi.Models;

namespace AeccApi.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailData emailData, EmailMessage emailMessage);
    }
}
