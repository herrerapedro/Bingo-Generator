using System;
namespace BingoUI
{
    public interface ICacheStore
    {
        Maybe<int> Read(int i);
        void Save(int i);
        void Clear();
    }
}
