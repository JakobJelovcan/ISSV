using System;
using System.Collections.Generic;
using System.Linq;

namespace ISSV.Core.Helpers
{
    public static class IEnumerableExtensions
    {
        public static TResult MaxOr<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult def)
        {
            return source.Any() ? source.Max(selector) : def;
        }
    }
}
