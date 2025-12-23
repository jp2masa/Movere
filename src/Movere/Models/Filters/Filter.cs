using System;
using System.IO;

namespace Movere.Models.Filters
{
    internal static class Filter
    {
        private static class Cache<T>
        {
            public static readonly IFilter<T> True = new FuncFilter<T>(_ => true);

            public static readonly IFilter<T> False = new FuncFilter<T>(_ => false);
        }

        public static class String
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

        public static class FileDialog
        {
            private static readonly Func<FileSystemEntry, FileDialogFilter, bool> s_matchesFilter =
                static (entry, filter) => entry is not File file || FileMatchesFilter(file, filter);

            public static IFilter<FileSystemEntry> Matches(FileDialogFilter? filter) =>
                filter is null || filter.Extensions.Contains("*")
                    ? True<FileSystemEntry>()
                    : StorageFuncFilter.New(filter, s_matchesFilter);

            private static bool FileMatchesFilter(File file, FileDialogFilter filter)
            {
                var extension = Path.GetExtension(file.Name);

                return !System.String.IsNullOrEmpty(extension)
                    && filter.Extensions.IndexOf(
                        extension.Substring(1),
                        0,
                        StringComparer.InvariantCultureIgnoreCase
                    ) != -1;
            }
        }

        public static IFilter<T> True<T>() => Cache<T>.True;

        public static IFilter<T> False<T>() => Cache<T>.False;
    }
}
