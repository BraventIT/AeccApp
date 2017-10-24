using System;
using System.Collections.Generic;
using System.Text;

namespace AeccApp.Core.Models
{
    public class BasePersonModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public object Telephone { get; set; }
        public string Province { get; set; }
    }
}
