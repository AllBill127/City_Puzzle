using System;
using System.Collections.Generic;
using System.Text;

namespace CityPuzzle.Classes
{
    public static class LinqLite
    {
        // Extention method that takes generic type and a delegate Func that is a bool condition
        // to return items from list suiting the condition
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

        // Extention method that takes a generic type and a delegate Func that takes generic type and returns a different generic type
        public static IEnumerable<TResult> Select<TResult, TSource>(this IEnumerable<TSource> items,
            Func<TSource, TResult> projection)
        {
            foreach (var item in items)
            {
                yield return projection(item);
            }
        }

        // Extention method to make a List out of IEnumerable items
        public static List<T> ToList<T>(this IEnumerable<T> items)
        {
            return new List<T>(items);
        }
    }
}
