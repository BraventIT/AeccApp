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
#if SERVICE
        [Display(Name = "Nombre")]
#endif
        public string Name { get; set; }
#if SERVICE
        [Display(Name = "Correo electrónico")]
#endif
        public string Email { get; set; }
#if SERVICE
        [Display(Name = "Teléfono")]
#endif
        public string Telephone { get; set; }
#if SERVICE
        [Display(Name = "Provincia")]
#endif
        public string Province { get; set; }

#if SERVICE
        [Display(Name = "Tipo de coordinador")]
#endif
        public RequestSourceEnum RequestSource { get; set; }

#if SERVICE
        [Display(Name = "Hospitales asignados")]
#endif
        public ICollection<HospitalAssignment> HospitalAssignments { get; set; }

    }
}
