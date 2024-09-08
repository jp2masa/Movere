// https://github.com/AvaloniaUI/Avalonia/blob/82d64089e15dca3712dc87dce757a29ccef2a04e/src/Avalonia.Base/Platform/Storage/FileIO/BclStorageFolder.cs

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Avalonia.Platform.Storage;

namespace Movere.Storage
{
    internal sealed class BclStorageFolder(DirectoryInfo directoryInfo)
        : BclStorageItem(directoryInfo), IStorageBookmarkFolder
    {
        public IAsyncEnumerable<IStorageItem> GetItemsAsync() => GetItemsCore(directoryInfo)
            .Select(WrapFileSystemInfo)
            .Where(f => f is not null)
            .AsAsyncEnumerable()!;

        public Task<IStorageFile?> CreateFileAsync(string name) => Task.FromResult(
            (IStorageFile?)WrapFileSystemInfo(CreateFileCore(directoryInfo, name)));

        public Task<IStorageFolder?> CreateFolderAsync(string name) => Task.FromResult(
            (IStorageFolder?)WrapFileSystemInfo(CreateFolderCore(directoryInfo, name)));
    }
}
