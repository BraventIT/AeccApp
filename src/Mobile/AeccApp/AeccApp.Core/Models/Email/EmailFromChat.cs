using Aecc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AeccApp.Core.Models
{
    public class EmailFromChat : EmailMessage
    {
        protected GlobalSetting GSetting { get { return GlobalSetting.Instance; } }

        public EmailFromChat(UserData counterpartUser, IEnumerable<Message> conversation, int rating)
        {
            To = GSetting.EmailChatAddress;
            Subject = $"Valoración de chat reportada por {GSetting.User?.FirstName} {GSetting.User?.Surname}";
            Body = GetEmailBody(counterpartUser, conversation, rating);
        }

        public string GetEmailBody(UserData counterpartUser, IEnumerable<Message> conversation, int rating)
        {
            return new StringBuilder(GSetting.EmailChatTemplate)
                .Replace("%UserName%", GSetting.User?.FirstName)
                .Replace("%UserSurname%", GSetting.User?.Surname)
                .Replace("%CounterpartName%", counterpartUser?.FirstName)
                .Replace("%CounterpartSurname%", counterpartUser?.Surname)
                .Replace("%ChatRating%", rating.ToString())
                .Replace("%Conversation%", conversation.Select(c => c.ToString()).Aggregate((i, j) => string.Concat(i, "\n", j)))
                .ToString();
        }
    }
}
