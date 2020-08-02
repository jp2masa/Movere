using System;

namespace Movere.Models.Filters
{
    internal static class FilterExtensions
    {
        private static class Cache<T>
        {
            public static readonly Func<T, (IFilter<T> f1, IFilter<T> f2), bool> And = (x, p) => p.f1.Matches(x) && p.f2.Matches(x);

            public static readonly Func<T, (IFilter<T> f1, IFilter<T> f2), bool> Or = (x, p) => p.f1.Matches(x) || p.f2.Matches(x);

            public static readonly Func<T, IFilter<T>, bool> Not = (x, f) => !f.Matches(x);
        }

        private static class Cache<T1, T2>
        {
            public static Func<T2, (IFilter<T1> f, Func<T2, T1> conv), bool> Cast = (value, p) => p.f.Matches(p.conv(value));
        }

        public static IFilter<T> And<T>(this IFilter<T> filter1, IFilter<T> filter2) =>
            StorageFuncFilter.New((filter1, filter2), Cache<T>.And);

        public static IFilter<T> Or<T>(this IFilter<T> filter1, IFilter<T> filter2) =>
            StorageFuncFilter.New((filter1, filter2), Cache<T>.Or);

        public static IFilter<T> Not<T>(this IFilter<T> filter) =>
            StorageFuncFilter.New(filter, Cache<T>.Not);

        public static IFilter<TTo> Cast<TFrom, TTo>(this IFilter<TFrom> filter, Func<TTo, TFrom> conv) =>
            StorageFuncFilter.New((filter, conv), Cache<TFrom, TTo>.Cast);

        public static Func<T, bool> ToFunc<T>(this IFilter<T> filter) => filter.Matches;
    }
}
