using System;
using System.Collections.Generic;
using System.Text;

namespace AeccApp.Core.Models
{
    public class RequestTypeModel
    {
        public Guid Id { get; set; }
        public RequestSourceEnum Source { get; set; }
        public string Name { get; set; }
    }

    public enum RequestSourceEnum
    {
        Hospital,
        Domicilio
    }
}
