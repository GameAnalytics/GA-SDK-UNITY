using System;
using System.Collections;
using System.Collections.Generic;

namespace GameAnalyticsSDK.Utilities
{
    public static class GA_HashtablePool
    {
        private static readonly Queue<Hashtable> _cache = new Queue<Hashtable>();

        public static IDisposable Get(out Hashtable result)
        {
            Pop(out result);
            return new Cached(result);
        }

        private static void Pop(out Hashtable result)
        {
            if (_cache.Count > 0)
            {
                result = _cache.Dequeue();
            }
            else
            {
                result = new Hashtable();
            }
        }

        private static void Push(Hashtable hashtable)
        {
            if (hashtable == null)
                return;

            if (_cache.Contains(hashtable))
                return;

            hashtable.Clear();
            _cache.Enqueue(hashtable);
        }

        private struct Cached : IDisposable
        {
            public Hashtable hashtable;

            public Cached(Hashtable hashtable)
            {
                this.hashtable = hashtable;
            }

            public void Dispose()
            {
                Push(hashtable);
            }
        }
    }
}
