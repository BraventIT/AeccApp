using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AeccApi.Models
{
    [JsonObject(IsReference = true)]
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
    }
}
