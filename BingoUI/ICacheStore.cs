using System;
namespace BingoUI
{
    public interface ICacheStore
    {
        bool Exists(int i);
        void Save(int i);
        void Clear();
    }
}
