using Aecc.Models;
using System;
using System.Collections.Generic;

namespace AeccApp.Core.Models
{
    public class UserData
    {
        public string Id { get; set; }
       
        public string Email { get; set; }
       
        public string Image { get; set; }

        public string FirstName { get; set; }
        public string Surname { get; set; }

        public string Name { get; set; }

        //public object Telephone { get; set; }
        //public string Address { get; set; }
        //public string PostalCode { get; set; }
        //public string City { get; set; }
        //public string Province { get; set; }

        public int? Age { get; set; }
        public string Gender { get; set; }

        public bool? IsMen
        {
            get
            {
                if (Gender?.StartsWith("h", StringComparison.CurrentCultureIgnoreCase) ?? false) return true;
                if (Gender?.StartsWith("m", StringComparison.CurrentCultureIgnoreCase) ?? false) return false;
                return null;
            }
        }

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
            Gender = party.ChannelAccount.Gender;

            PartyId = party.ToJsonString();
        }
    }
}
