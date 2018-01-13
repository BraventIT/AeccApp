using System;
using System.Collections.Generic;
using System.Linq;

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

        public static void SyncExact<T>(this IList<T> oc, IList<T> source)
        {
            if (!source.Any())
            {
                oc.Clear();
                return;
            }

            for (int i = 0; i < source.Count; i++)
            {
                var item = source[i];
                if (oc.Count > i)
                {
                    if (!item.Equals(oc[i]))
                    {
                        oc.RemoveAt(i);
                        oc.Insert(i, item);
                    }
                }
                else
                {
                    oc.Add(item);
                }
            }

            while (oc.Count != source.Count)
            {
                oc.RemoveAt(oc.Count - 1);
            }
        }
    }
}
