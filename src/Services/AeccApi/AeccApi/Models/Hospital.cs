using Newtonsoft.Json;
using System.Collections.Generic;
#if WEB
using System.ComponentModel.DataAnnotations;
#endif
namespace Aecc.Models
{
    [JsonObject(IsReference = true)]
    public class Hospital
    {
        public int ID { get; set; }
#if WEB
        [Display(Name = "Nombre")]
#endif
        public string Name { get; set; }
        public string Street { get; set; }
//#if WEB
//        [Display(Name = "Ciudad")]
//#endif
//        public string City { get; set; }
#if WEB
        [Display(Name = "Provincia")]
#endif
        public string Province { get; set; }

        public ICollection<HospitalAssignment> HospitalAssignments { get; set; }
    }
}
