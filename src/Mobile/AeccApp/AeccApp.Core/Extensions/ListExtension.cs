﻿using System;
using System.Collections.Generic;

namespace AeccApp.Core.Extensions
{
    public static class ListExtension
    {
        public static void AddRange<T>(this IList<T> oc, IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            foreach (var item in collection)
            {
                oc.Add(item);
            }
        }
    }
}
