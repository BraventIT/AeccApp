﻿using Aecc.Models;
using System;
using System.Collections.Generic;

namespace AeccApp.Core.Models
{
    public class UserData : IEquatable<UserData>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Image { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }


        private string _description;

        public string Description
        {
            get { return _description == null || _description.Equals(string.Empty) ? $"Hola, soy {Name} " : _description; }
            set { _description = value; }
        }

        public string DisplayDescription
        {
            get
            {
                return (!Age.HasValue) ? (string.IsNullOrEmpty(Description)) ?
                    "-" :
                    $"{Description}" :
                    $"{Age} años - {Description}";
            }
        }

        public int? Age { get; set; }
        public string Gender { get; set; }

        public string DisplayGender
        {
            get
            {
                if (Gender != null)
                {
                    switch (Gender.ToLower())
                    {
                        case "m":
                            return "Mujer";
                        case "h":
                            return "Hombre";
                        default:
                            return "No especificado";
                    }
                }
                return "No especificado";
            }
        }


        public bool? IsMan
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

        public bool Equals(UserData other)
        {
            return other.PartyId == PartyId;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as UserData);
        }

        public override int GetHashCode()
        {
            return PartyId.GetHashCode();
        }
    }
}
