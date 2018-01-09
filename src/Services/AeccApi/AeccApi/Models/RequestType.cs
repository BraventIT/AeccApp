#if !APP
using System.ComponentModel.DataAnnotations;
#endif

using System;

namespace Aecc.Models
{
    public class RequestType : IEquatable<RequestType>
    {
        public int Id { get; set; }
#if !APP
        [Display(Name = "Origen")]
#endif
        public RequestSourceEnum Source { get; set; }
#if !APP
        [Display(Name = "Nombre")]
#endif
        public string Name { get; set; }


        public bool Equals(RequestType other)
        {
            return other.Id == Id
                        && other.Source == Source
                        && other.Name == Name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RequestType);
        }

        public override int GetHashCode()
        {
            return new {Id,Source,Name}.GetHashCode();
        }
    }
}
