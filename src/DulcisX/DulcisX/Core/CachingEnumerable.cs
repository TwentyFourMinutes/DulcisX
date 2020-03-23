using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DulcisX.Core
{
    internal class CachingEnumerable<T> : IEnumerable<T>
    {
        private readonly ConcurrentDictionary<int, T> _cache = new ConcurrentDictionary<int, T>();
        private readonly IEnumerator<T> _baseEnumerator;
        private readonly object _iterationLock = new object();
        internal CachingEnumerable(IEnumerator<T> baseEnumerable)
        {
            _baseEnumerator = baseEnumerable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var cachedItem in _cache)
            {
                yield return cachedItem.Value;
            }

            while (_baseEnumerator.MoveNext())
            {
                lock (_iterationLock)
                {
                    var current = _baseEnumerator.Current;

                    _cache.TryAdd(_cache.Count, current);

                    yield return current;
                }
            }

            _baseEnumerator.Dispose();
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
