using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AeccApi.Models
{
    public class NewsOptions
    {
        public string UrlBase { get; set; }
        public string UrlNews { get; set; }
        public short NumNewsToLoad { get; set; }
    }
}
