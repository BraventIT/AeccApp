using System;
using System.Collections.Generic;
using System.Text;

namespace AeccApp.Core.Extensions
{
    public static class StringCompareExtension
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source != null && toCheck != null && source.IndexOf(toCheck, comp) >= 0;
        }
    }
}
