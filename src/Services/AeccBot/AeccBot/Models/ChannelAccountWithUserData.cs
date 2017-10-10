#if SERVICE
using Microsoft.Bot.Connector;
#else
using Microsoft.Bot.Connector.DirectLine;
#endif
using Newtonsoft.Json;

namespace AeccBot.Models
{
    public class ChannelAccountWithUserData : ChannelAccount
    {
        public ChannelAccountWithUserData()
        {

        }

        public ChannelAccountWithUserData(string id = null, string name = null)
        {
            Id = id;
            Name = name;
        }

        public ChannelAccountWithUserData(ChannelAccount account)
        {
#if SERVICE
            Properties = account.Properties;
            FirstName = Properties?[nameof(FirstName)]?.ToString();
            Surname = Properties?[nameof(Surname)]?.ToString();
            Email = Properties?[nameof(Email)]?.ToString();
#endif
            Id = account.Id;
            Name = account.Name;
        }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public int Age { get; set; }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static ChannelAccountWithUserData FromJsonString(string jsonString)
        {
            return JsonConvert.DeserializeObject<ChannelAccountWithUserData>(jsonString);
        }
    }
}