// https://github.com/AvaloniaUI/Avalonia/blob/92fdbb5152f11f707b3f46cda25472525c70c3a3/src/Avalonia.Base/Platform/Storage/FileIO/StorageProviderHelpers.cs

using System;
using System.IO;
using System.Linq;
using System.Text;

using Avalonia.Platform.Storage;

namespace Movere.Storage
{
    internal static class StorageProviderHelpers
    {
        public static Uri FilePathToUri(string path)
        {
            var uriPath = new StringBuilder(path)
                .Replace("%", $"%{(int)'%':X2}")
                .Replace("[", $"%{(int)'[':X2}")
                .Replace("]", $"%{(int)']':X2}")
                .ToString();

            return new UriBuilder("file", string.Empty) { Path = uriPath }.Uri;
        }
    
        public static string NameWithExtension(string path, string? defaultExtension, FilePickerFileType? filter)
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
    }
}
