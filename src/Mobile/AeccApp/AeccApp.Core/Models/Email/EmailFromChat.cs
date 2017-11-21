using System;
using System.Collections.Generic;
using System.Text;

namespace AeccApp.Core.Models.Email
{
    public class EmailFromChat : EmailBase
    {
        protected GlobalSetting GSetting { get { return GlobalSetting.Instance; } }

        public EmailFromChat(UserData counterpartUser, IList<Message> conversation, int rating)
        {
            To = GSetting.EmailChatAddress;
            Subject = $"Valoración de chat reportada {GSetting.User?.Name} {GSetting.User?.Surname}";
            Body = GSetting.EmailChatTemplate?.GetEmailBody(GSetting.User, counterpartUser, conversation, rating);
        }


    }
}
