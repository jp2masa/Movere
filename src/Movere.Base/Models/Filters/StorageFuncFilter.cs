using System;

namespace Movere.Models.Filters
{
    internal static class StorageFuncFilter
    {
        public static StorageFuncFilter<T, TStorage> New<T, TStorage>(TStorage storage, Func<T, TStorage, bool> match) =>
            new StorageFuncFilter<T, TStorage>(storage, match);
    }

    internal sealed class StorageFuncFilter<T, TStorage>(
        TStorage storage,
        Func<T, TStorage, bool> matches
    )
        : IFilter<T>
    {
        private readonly TStorage _storage = storage;
        private readonly Func<T, TStorage, bool> _matches = matches;

        public bool Matches(T value) => _matches(value, _storage);
    }
}
