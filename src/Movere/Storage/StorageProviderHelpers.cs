// https://github.com/AvaloniaUI/Avalonia/blob/82d64089e15dca3712dc87dce757a29ccef2a04e/src/Avalonia.Base/Platform/Storage/FileIO/StorageProviderHelpers.cs

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Avalonia.Platform.Storage;

namespace Movere.Storage
{
    internal static class StorageProviderHelpers
    {
        public static BclStorageItem? TryCreateBclStorageItem(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                var directory = new DirectoryInfo(path);
                if (directory.Exists)
                {
                    return new BclStorageFolder(directory);
                }

                var file = new FileInfo(path);
                if (file.Exists)
                {
                    return new BclStorageFile(file);
                }
            }

            return null;
        }

        public static string? TryGetPathFromFileUri(Uri? uri)
        {
            // android "content:", browser and ios relative links are ignored.
            return uri is { IsAbsoluteUri: true, Scheme: "file" } ? uri.LocalPath : null;
        }

        public static Uri UriFromFilePath(string path, bool isDirectory)
        {
            var uriPath = new StringBuilder(path)
                .Replace("%", $"%{(int)'%':X2}")
                .Replace("[", $"%{(int)'[':X2}")
                .Replace("]", $"%{(int)']':X2}");

            if (!path.EndsWith('/') && isDirectory)
            {
                uriPath.Append('/');
            }

            return new UriBuilder("file", string.Empty) { Path = uriPath.ToString() }.Uri;
        }

        public static Uri? TryGetUriFromFilePath(string path, bool isDirectory)
        {
            try
            {
                return UriFromFilePath(path, isDirectory);
            }
            catch
            {
                return null;
            }
        }

        [return: NotNullIfNotNull(nameof(path))]
        public static string? NameWithExtension(string? path, string? defaultExtension, FilePickerFileType? filter)
        {
            var name = Path.GetFileName(path);
            if (name != null && !Path.HasExtension(name))
            {
                if (filter?.Patterns?.Count > 0)
                {
                    if (defaultExtension != null
                        && filter.Patterns.Contains(defaultExtension))
                    {
                        return Path.ChangeExtension(path, defaultExtension.TrimStart('.'));
                    }

                    var ext = filter.Patterns.FirstOrDefault(x => x != "*.*");
                    ext = ext?.Split(new[] { "*." }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                    if (ext != null)
                    {
                        return Path.ChangeExtension(path, ext);
                    }
                }

                if (defaultExtension != null)
                {
                    return Path.ChangeExtension(path, defaultExtension);
                }
            }

            return path;
        }

        // https://github.com/AvaloniaUI/Avalonia/blob/82d64089e15dca3712dc87dce757a29ccef2a04e/src/Shared/StringCompatibilityExtensions.cs#L12-L14
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool EndsWith(this string str, char search) =>
            str.Length > 0 && str[str.Length - 1] == search;
    }
}
