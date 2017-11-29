using System;
using System.Collections.Generic;
using System.Text;

namespace AeccApp.Core.Models
{
    public class ChatFiltersModel
    {
        public ChatFiltersModel(int minimumAge, int maximumAge, string gender)
        {
            this.MinimumAge = minimumAge;
            this.MaximumAge = maximumAge;
            this.Gender = gender;
        }
        public int MinimumAge { get; set; } 
        public int MaximumAge { get; set; }
        public string Gender { get; set; }
    }
}
