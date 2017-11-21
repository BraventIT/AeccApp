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
                return (Activity != null) ? (Activity.From == null || Activity.From.Id == GlobalSetting.Instance.User.Id) ?
                       MessageType.Sent : MessageType.Received : MessageType.Time;
            }
        }

        public DateTime DateTime { get; set; }

        public override string ToString()
        {
            return string.Concat(DateTime.ToLocalTime().ToString("dd MMM yyy HH:mm:ss"),
                " - ", Activity.Text);

        }
    }
}
