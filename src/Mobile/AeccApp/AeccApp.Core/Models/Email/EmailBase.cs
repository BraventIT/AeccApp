using Aecc.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AeccApp.Core.Models.Email
{
   public class EmailBase : EmailMessage
    {
        public RequestModel SentRequest { get; set; }
    }
}
