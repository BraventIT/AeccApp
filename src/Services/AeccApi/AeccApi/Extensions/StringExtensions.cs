using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AeccApi.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source != null && toCheck != null && source.IndexOf(toCheck, comp) >= 0;
        }
    }
}
