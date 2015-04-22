using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoUI
{
    public class CacheStore : ICacheStore
    {
        private readonly ConcurrentBag<int> values;

        public CacheStore()
        {
            this.values = new ConcurrentBag<int>();
        }

        public bool Exists(int i)
        {
            return this.values.Contains(i);
        }

        public void Save(int i)
        {
            this.values.Add(i);
        }

        public void Clear()
        {
            int someInt;
            while (!this.values.IsEmpty)
            {
                this.values.TryTake(out someInt);
            }
        }
    }
}
