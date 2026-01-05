using System;
using System.IO;

namespace Movere.Models.Filters
{
    internal static class FileDialogFilterExtensions
    {
        private static readonly Func<FileSystemEntry, FileDialogFilter, bool> s_matchesFilter =
            static (entry, filter) => entry is not File file || FileMatchesFilter(file, filter);

        public static IFilter<FileSystemEntry> AsFilter(this FileDialogFilter? filter) =>
            filter is null || filter.Extensions.Contains("*")
                ? Filter.True<FileSystemEntry>()
                : StorageFuncFilter.New(filter, s_matchesFilter);

        private static bool FileMatchesFilter(File file, FileDialogFilter filter)
        {
            var extension = Path.GetExtension(file.Name);

            return !String.IsNullOrEmpty(extension)
                && filter.Extensions.IndexOf(
                    extension.Substring(1),
                    0,
                    StringComparer.InvariantCultureIgnoreCase
                ) != -1;
        }
    }
}
