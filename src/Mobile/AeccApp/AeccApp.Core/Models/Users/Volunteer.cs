using AeccBot.Models;

namespace AeccApp.Core.Models
{
    public class Volunteer
    {
        public string Image { get; set; }

        public string Name { get; set; }

        public string Age { get; set; }

        public string Gender { get; set; }

        public string Description { get; set; }

        public string PartyId { get; set; }

        public Volunteer(Party party)
        {
            Name = party.ChannelAccount.Name;
            PartyId = party.ToJsonString();            
        }
    }
}
