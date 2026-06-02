using System;

namespace Movere.Models.Filters
{
    internal static class StringFilter
    {
        private static readonly Func<string, char, bool> s_containsChar =
            static (x, c) => x.IndexOf(c) != -1;

        private static readonly Func<string, (string str, StringComparison cmp), bool> s_containsString =
            static (x, p) => x.IndexOf(p.str, p.cmp) != -1;

        private static readonly Func<string, (string str, StringComparison cmp), bool> s_equalsString =
            static (x, p) => x.Equals(p.str, p.cmp);

        public static IFilter<string> Contains(char c) => StorageFuncFilter.New(c, s_containsChar);

        public static IFilter<string> Contains(string str, StringComparison stringComparison) =>
            StorageFuncFilter.New((str, stringComparison), s_containsString);

        public static IFilter<string> Equals(string str, StringComparison stringComparison) =>
            StorageFuncFilter.New((str, stringComparison), s_equalsString);
    }
}
