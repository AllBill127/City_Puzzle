using System;
using System.Collections.Generic;
using System.Text;

namespace CityPuzzle.Classes
{
    public static class LinqLite
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> items, 
            Func<T, bool> condition)
        {
            foreach (var item in items)
            {
                if (condition(item))
                {
                    yield return item; 
                }
            }
        }

        public static IEnumerable<TResult> Select<TResult, TSource>(this IEnumerable<TSource> items,
            Func<TSource, TResult> projection)
        {
            foreach (var item in items)
            {
                yield return projection(item);
            }
        }

        public static List<T> ToList<T>(this IEnumerable<T> items)
        {
            return new List<T>(items);
        }
    }
}
