using System;

namespace Movere.Models.Filters
{
    internal sealed class FuncFilter<T> : IFilter<T>
    {
        private readonly Func<T, bool> _matches;

        public FuncFilter(Func<T, bool> matches)
        {
            _matches = matches;
        }

        public bool Matches(T value) => _matches(value);
    }
}
