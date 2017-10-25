using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AeccApi.Models
{
    public class RequestType
    {
        public int Id { get; set; }
        public RequestSourceEnum Source { get; set; }
        public string Name { get; set; }
    }
}
