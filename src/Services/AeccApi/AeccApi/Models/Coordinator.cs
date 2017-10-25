using Newtonsoft.Json;
using System.Collections.Generic;
#if SERVICE
using System.ComponentModel.DataAnnotations;
#endif

namespace AeccApi.Models
{
    [JsonObject(IsReference = true)]
    public class Coordinator
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Province { get; set; }
#if SERVICE
        [Display(Name ="Empleo")]
#endif
        public ICollection<Employ> Employments { get; set; }

    }
}
