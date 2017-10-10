using System;
using System.Collections.Generic;
using System.Text;

namespace AeccApp.Core.Messages
{
    public class ChatStateMessage
    {
        public bool VolunteerIsActive { get; set; }

        public bool InConversation { get; set; }

        public ChatStateMessage(bool volunteerIsActive, bool inConversation)
        {
            VolunteerIsActive = volunteerIsActive;
            InConversation = inConversation;
        }
    }
}
