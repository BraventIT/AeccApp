using Newtonsoft.Json;

namespace Aecc.Models
{
    [JsonObject(IsReference = true)]
    public class HospitalAssignment
    {
        public int HospitalID { get; set; }
        public int CoordinatorID { get; set; }

        public Hospital Hospital { get; set; }
        public Coordinator Coordinator { get; set; }
    }
}
