using Newtonsoft.Json;
using System.Collections.Generic;
#if !APP
using System.ComponentModel.DataAnnotations;
#endif
namespace Aecc.Models
{
    [JsonObject(IsReference = true)]
    public class Hospital
    {
        public int ID { get; set; }
#if !APP
        [Display(Name = "Nombre")]
#endif
        public string Name { get; set; }
#if !APP
        [Display(Name = "Dirección")]
#endif
        public string Street { get; set; }
//#if !APP
//        [Display(Name = "Ciudad")]
//#endif
//        public string City { get; set; }
#if !APP
        [Display(Name = "Provincia")]
#endif
        public string Province { get; set; }

        public ICollection<HospitalAssignment> HospitalAssignments { get; set; }
    }
}
