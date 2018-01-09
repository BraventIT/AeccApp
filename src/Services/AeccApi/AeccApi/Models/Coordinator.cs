using Newtonsoft.Json;
using System.Collections.Generic;
#if !APP
using System.ComponentModel.DataAnnotations;
#endif

namespace Aecc.Models
{
    [JsonObject(IsReference = true)]
    public class Coordinator
    {
        public int ID { get; set; }
#if !APP
        [Display(Name = "Nombre")]
#endif
        public string Name { get; set; }
#if !APP
        [Display(Name = "Correo electrónico")]
#endif
        public string Email { get; set; }
#if !APP
        [Display(Name = "Teléfono")]
#endif
        public string Telephone { get; set; }
#if !APP
        [Display(Name = "Provincia")]
#endif
        public string Province { get; set; }

#if !APP
        [Display(Name = "Tipo de coordinador")]
#endif
        public RequestSourceEnum RequestSource { get; set; }

#if !APP
        [Display(Name = "Hospitales asignados")]
#endif
        public ICollection<HospitalAssignment> HospitalAssignments { get; set; }

    }
}
