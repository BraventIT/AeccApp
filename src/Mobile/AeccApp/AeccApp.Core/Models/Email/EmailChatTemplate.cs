using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AeccApp.Core.Models.Email
{
    public class EmailChatTemplate
    {
        private readonly string _template;
        
        public EmailChatTemplate(string template)
        {
            this._template = template;
        }

        public string GetEmailBody(UserData user, UserData counterpartUser, IList<Message> conversation, int rating)
        {
            return new StringBuilder(_template)
                .Replace("%UserName%", user?.Name)
                .Replace("%UserSurname%", user?.Surname)
                .Replace("%CounterpartName%", counterpartUser?.Name)
                .Replace("%CounterpartSurname%", counterpartUser?.Surname)
                .Replace("%ChatRating%", rating.ToString())
                .Replace("%Conversation%", conversation.Select(c => c.ToString()).Aggregate((i, j) => string.Concat(i, "\n", j)))
                .ToString();
        }
        
    }
}
