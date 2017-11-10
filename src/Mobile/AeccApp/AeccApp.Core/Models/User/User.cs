using Aecc.Models;
using System.Collections.Generic;

namespace AeccApp.Core.Models
{
    public class UserData: BasePersonModel
    {
        public string UserId { get; set; }
        public bool IsVolunteer { get; set; }
       
        public string Description { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
       
        public int Age { get; set; }
        public List<string> Groups { get; set; }

        public string PartyId { get; set; }


        public UserData()
        {

        }

        public UserData(Party party)
        {
            if (party == null)
                return;

            Name = party.ChannelAccount.Name;
            FirstName = party.ChannelAccount.FirstName;
            Surname = party.ChannelAccount.Surname;
            Email = party.ChannelAccount.Email;
            Age = party.ChannelAccount.Age;

            PartyId = party.ToJsonString();
        }
    }
}
