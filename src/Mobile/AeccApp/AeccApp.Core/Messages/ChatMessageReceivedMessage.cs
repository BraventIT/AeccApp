using AeccApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Messages
{
    public class ChatMessageReceivedMessage
    {
        public Message Message { get; set; }

        public ChatMessageReceivedMessage(Message message)
        {
            Message = message;
        }
    }
}
