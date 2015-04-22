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

        public Maybe<int> Read(int i)
        {
            if (this.values.Contains(i))
                return new Maybe<int>(i);
            return new Maybe<int>();
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
