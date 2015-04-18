using System;
using System.Collections.Generic;
using System.Linq;

namespace GoldenTicket.Utilities
{
    public static class Extensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }

        public static string ToCustomLower(this string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : value.ToLower();
        }
    }
}
