using Newtonsoft.Json;

namespace AeccApi.Models
{
    [JsonObject(IsReference = true)]
    public class Employ
    {
        public int ID { get; set; }
        public int HospitalID { get; set; }
        public int CoordinatorID { get; set; }
        public RequestSourceEnum RequestSource { get; set; }

        public Hospital Hospital { get; set; }
        public Coordinator Coordinator { get; set; }
    }
}
