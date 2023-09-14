using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecAlliance.Service1.Tests
{
    public static class MockMemoryCacheService
    {
        public static IMemoryCache GetMemoryCache(object expectedValue)
        {
            var mockMemoryCache = new Mock<IMemoryCache>();
            mockMemoryCache
                .Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue))
                .Returns(true);
            return mockMemoryCache.Object;
        }

        public static object Get(this IMemoryCache cache, object key)
        {
            cache.TryGetValue(key, out object value);
            return value;
        }

        public static TItem Get<TItem>(this IMemoryCache cache, object key)
        {
            return (TItem)(cache.Get(key) ?? default(TItem));
        }

        public static bool TryGetValue<TItem>(this IMemoryCache cache, object key, out TItem value)
        {
            if (cache.TryGetValue(key, out object result))
            {
                if (result is TItem item)
                {
                    value = item;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public static TItem Set<TItem>(this IMemoryCache cache, object key, TItem value)
        {
            var entry = cache.CreateEntry(key);
            entry.Value = value;
            entry.Dispose();

            return value;
        }
    }
}
