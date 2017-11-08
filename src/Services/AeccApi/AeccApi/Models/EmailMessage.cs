#if SERVICE
using System.ComponentModel.DataAnnotations;
#endif

namespace AeccApi.Models
{
    public class EmailMessage
    {
#if SERVICE
        [Display(Name = "Asunto")]
#endif
        public string Subject { get; set; }

#if SERVICE
        [Display(Name = "Cuerpo")]
#endif
        public string Body { get; set; }

#if SERVICE
        [Display(Name = "Destinatarios", Description = "Email destinatarios separados por ;")]
#endif
        public string To { get; set; }

#if SERVICE
        [Display(Name = "Peticion", Description = "Concatenado de datos de la peticion")]
#endif
    }
}
