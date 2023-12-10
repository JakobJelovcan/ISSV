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

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (T item in collection) action(item);
            return collection;
        }
    }

}
