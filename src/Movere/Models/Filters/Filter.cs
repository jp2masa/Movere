using System;
using System.IO;
using System.Linq;

namespace Movere.Models.Filters
{
    internal static class Filter
    {
        private static class Cache<T>
        {
            public static readonly IFilter<T> True = new FuncFilter<T>(x => true);

            public static readonly IFilter<T> False = new FuncFilter<T>(x => false);
        }

        public static class String
        {
            private static readonly Func<string, char, bool> ContainsChar = (x, c) => x.Contains(c);

            private static readonly Func<string, (string str, StringComparison cmp), bool> ContainsString = (x, p) => x.IndexOf(p.str, p.cmp) != -1;

            private static readonly Func<string, (string str, StringComparison cmp), bool> EqualsString = (x, p) => x.Equals(p.str, p.cmp);

            public static IFilter<string> Contains(char c) => StorageFuncFilter.New(c, ContainsChar);

            public static IFilter<string> Contains(string str, StringComparison stringComparison) =>
                StorageFuncFilter.New((str, stringComparison), ContainsString);

            public static IFilter<string> Equals(string str, StringComparison stringComparison) =>
                StorageFuncFilter.New((str, stringComparison), EqualsString);
        }

        public static class FileDialog
        {
            private static readonly Func<FileSystemEntry, FileDialogFilter, bool> MatchesFilter =
                (entry, filter) => !(entry is File file) || FileMatchesFilter(file, filter);

            public static IFilter<FileSystemEntry> Matches(FileDialogFilter? filter)
            {
                if (filter == null || filter.Extensions.Contains("*"))
                {
                    return True<FileSystemEntry>();
                }

                return StorageFuncFilter.New(filter, MatchesFilter);
            }

            private static bool FileMatchesFilter(File file, FileDialogFilter filter)
            {
                var extension = Path.GetExtension(file.Name);
                return extension != null && filter.Extensions.Contains(extension.Substring(1), StringComparer.InvariantCultureIgnoreCase);
            }
        }

        public static IFilter<T> True<T>() => Cache<T>.True;

        public static IFilter<T> False<T>() => Cache<T>.False;
    }
}
