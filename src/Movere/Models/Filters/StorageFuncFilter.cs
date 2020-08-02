using System;

namespace Movere.Models.Filters
{
    internal static class StorageFuncFilter
    {
        public static StorageFuncFilter<T, TStorage> New<T, TStorage>(TStorage storage, Func<T, TStorage, bool> match) =>
            new StorageFuncFilter<T, TStorage>(storage, match);
    }

    internal sealed class StorageFuncFilter<T, TStorage> : IFilter<T>
    {
        private readonly TStorage _storage;
        private readonly Func<T, TStorage, bool> _matches;

        public StorageFuncFilter(TStorage storage, Func<T, TStorage, bool> matches)
        {
            _storage = storage;
            _matches = matches;
        }

        public bool Matches(T value) => _matches(value, _storage);
    }
}
