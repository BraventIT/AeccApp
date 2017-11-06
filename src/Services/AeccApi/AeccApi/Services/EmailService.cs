using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using AeccApi.Models;

namespace AeccApi.Services
{
    public class EmailService : IEmailService
    {
        public Task SendAsync(EmailData emailData, EmailMessage emailMessage)
        {
            SmtpClient client = this.CreateSmtpClient(emailData);
            MailMessage mailMessage = this.CreateMailMessage(emailData, emailMessage);

            return client.SendMailAsync(mailMessage);
        }

        private SmtpClient CreateSmtpClient(EmailData emailData)
        {
            return new SmtpClient()
            {
                Host = emailData.Host,
                Port = emailData.Port,
                EnableSsl = true,
                Credentials = new NetworkCredential(emailData.Address, emailData.Password)
            };
        }

        private MailMessage CreateMailMessage(EmailData emailData, EmailMessage emailMessage)
        {
            MailMessage mailMessage =  new MailMessage()
            {
                From = new MailAddress(emailData.Address),
                Body = emailMessage.Body,
                Subject = emailMessage.Subject,
            };
            emailMessage.To.Split(";").ToList().ForEach(e => mailMessage.To.Add(e));
            return mailMessage;
        }
    }
}
