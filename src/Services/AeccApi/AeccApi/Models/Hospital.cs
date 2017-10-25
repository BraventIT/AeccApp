using Newtonsoft.Json;
using System.Collections.Generic;
#if SERVICE
using System.ComponentModel.DataAnnotations;
#endif
namespace AeccApi.Models
{
    [JsonObject(IsReference = true)]
    public class Hospital
    {
        public int ID { get; set; }
#if SERVICE
        [Display(Name = "Nombre")]
#endif
        public string Name { get; set; }
        public string Street { get; set; }
#if SERVICE
        [Display(Name = "Ciudad")]
#endif
        public string City { get; set; }
#if SERVICE
        [Display(Name = "Provincia")]
#endif
        public string Province { get; set; }

        public ICollection<Employ> Employees { get; set; }
    }
}
