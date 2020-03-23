using System.Collections.Generic;

namespace DulcisX.Core.Extensions
{
    internal static class EnumerableExtensions
    {
        internal static IEnumerable<T> ToCachingEnumerable<T>(this IEnumerable<T> enumerable)
            => new CachingEnumerable<T>(enumerable.GetEnumerator());
    }
}
