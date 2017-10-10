using AeccBot.MessageRouting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Messages
{
    public class ChatEventMessage
    {
        public MessageRouterResultType Type { get; set; }

        public string Message { get; set; }

        public string MessageTitle { get; set; }


        public ChatEventMessage(MessageRouterResultType type,  string message)
        {
            Type = type;
            Message = message;
        }
    }
}
