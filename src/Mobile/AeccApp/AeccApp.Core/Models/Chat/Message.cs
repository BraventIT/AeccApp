using Microsoft.Bot.Connector.DirectLine;
using System;

namespace AeccApp.Core.Models
{
    public class Message
    {
        public Activity Activity { get; set; }
        public MessageType MessageType
        {
            get
            {
                return (Activity.From == null || Activity.From.Id == GlobalSetting.Instance.User.UserId) ?
                     MessageType.Sent : MessageType.Received;
            }
        }

        public DateTime DateTime { get; set; }
        public string UserTime
        {
            get {
                switch (MessageType)
                {
                    case MessageType.Received:
                        return $"{Activity.From.Name} a las {DateTime.ToLocalTime().ToString("HH:mm")}";
                    case MessageType.Sent:
                        return $"Tú a las {DateTime.ToLocalTime().ToString("HH:mm")}";
                }

                return string.Empty;
            }
        }
    }
}
