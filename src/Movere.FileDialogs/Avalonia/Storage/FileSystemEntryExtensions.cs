using System.IO;

using Avalonia.Platform.Storage;

using Movere.Models;
using Movere.Storage;
using File = Movere.Models.File;

namespace Movere.Avalonia.Storage
{
    public static class FileSystemEntryExtensions
    {
        public static IStorageItem? ToStorageItem(this FileSystemEntry @this) =>
            @this switch
            {
                Folder folder =>
                    new BclStorageFolder(new DirectoryInfo(folder.FullPath)),
                File file =>
                    new BclStorageFile(new FileInfo(file.FullPath)),
                _ =>
                    null
            };

        public static IStorageFolder ToStorageFolder(this Folder @this) =>
            new BclStorageFolder(new DirectoryInfo(@this.FullPath));

        public static IStorageFile ToStorageFile(this File @this) =>
            new BclStorageFile(new FileInfo(@this.FullPath));
    }
}
