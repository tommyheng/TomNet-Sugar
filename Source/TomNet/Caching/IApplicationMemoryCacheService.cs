using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace TomNet.Caching
{
    public interface IApplicationMemoryCacheService<K, V>
    {
        void Set(K key, V value);
        V Get(K key);
    }

    public abstract class DictionaryMemoryCacheService<K, V> : IApplicationMemoryCacheService<K, V>
    {
        private readonly ConcurrentDictionary<K, V> _cache = new ConcurrentDictionary<K, V>();
        public void Set(K key, V value)
        {
            _cache[key] = value;
        }

        public V Get(K key)
        {
            var v = default(V);
            _cache.TryGetValue(key, out v);
            return v;
        }
    }
}
