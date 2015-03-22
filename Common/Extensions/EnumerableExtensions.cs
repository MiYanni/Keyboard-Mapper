using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Extensions
{
    /// <summary>
    /// Contains extension methods for Enumerables.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Replaces elements of a sequence that satisfy the predicate into a new form.
        /// Elements that do not satisfy the predicate remain unchanged.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="replacer">A function that dictates how the satisfactory elements are to be replaced.</param>
        /// <returns>An IEnumerable whose elements are the result of invoking the replacement function on each satisfactory element of source.</returns>
        public static IEnumerable<TSource> Replace<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, TSource> replacer)
        {
            return source.ThrowIfNull().Select(s => predicate(s) ? replacer(s) : s);
        }

        /// <summary>
        /// Replaces elements of a sequence that satisfy the predicate into a new form.
        /// Elements that do not satisfy the predicate remain unchanged.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="predicate">A function to test each element for a condition. The second parameter of the function represents the index of the source element.</param>
        /// <param name="replacer">A function that dictates how the satisfactory elements are to be replaced. The second parameter of the function represents the index of the source element.</param>
        /// <returns>An IEnumerable whose elements are the result of invoking the replacement function on each satisfactory element of source.</returns>
        public static IEnumerable<TSource> Replace<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate, Func<TSource, int, TSource> replacer)
        {
            return source.ThrowIfNull().Select((s, i) => predicate(s, i) ? replacer(s, i) : s);
        }
    }
}