using System.ComponentModel.DataAnnotations;

namespace Aecc.Models
{
    public class RequestType
    {
        public int Id { get; set; }
#if WEB
        [Display(Name = "Origen")]
#endif
        public RequestSourceEnum Source { get; set; }
#if WEB
        [Display(Name = "Nombre")]
#endif
        public string Name { get; set; }
    }
}
