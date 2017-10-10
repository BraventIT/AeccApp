using AeccApp.Core.Models;
using AeccBot.MessageRouting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Messages
{
    public class ChatEngagementEventMessage
    {
        public string RequestPartyId { get; set; }

        public ChatEngagementEventMessage(string requestPartyId)
        {
            RequestPartyId = requestPartyId;
        }
    }
}
