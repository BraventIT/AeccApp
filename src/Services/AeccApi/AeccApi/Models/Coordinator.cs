using Newtonsoft.Json;
using System.Collections.Generic;
#if WEB
using System.ComponentModel.DataAnnotations;
#endif

namespace AeccApi.Models
{
    [JsonObject(IsReference = true)]
    public class Coordinator
    {
        public int ID { get; set; }
#if WEB
        [Display(Name = "Nombre")]
#endif
        public string Name { get; set; }
#if WEB
        [Display(Name = "Correo electrónico")]
#endif
        public string Email { get; set; }
#if WEB
        [Display(Name = "Teléfono")]
#endif
        public string Telephone { get; set; }
#if WEB
        [Display(Name = "Provincia")]
#endif
        public string Province { get; set; }

#if WEB
        [Display(Name = "Tipo de coordinador")]
#endif
        public RequestSourceEnum RequestSource { get; set; }

#if WEB
        [Display(Name = "Hospitales asignados")]
#endif
        public ICollection<HospitalAssignment> HospitalAssignments { get; set; }

    }
}
