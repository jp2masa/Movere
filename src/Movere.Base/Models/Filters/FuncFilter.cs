using System;

namespace Movere.Models.Filters
{
    internal sealed class FuncFilter<T>(Func<T, bool> matches) : IFilter<T>
    {
        private readonly Func<T, bool> _matches = matches;

        public bool Matches(T value) => _matches(value);
    }
}
