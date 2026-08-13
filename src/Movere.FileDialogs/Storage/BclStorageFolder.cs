// https://github.com/AvaloniaUI/Avalonia/blob/e33eaed9c106846b200680751022385d9cc5dc6f/src/Avalonia.Base/Platform/Storage/FileIO/BclStorageFolder.cs

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

        public Task<IStorageFolder?> GetFolderAsync(string name) => Task.FromResult(
            (IStorageFolder?)WrapFileSystemInfo(GetFolderCore(directoryInfo, name)));

        public Task<IStorageFile?> GetFileAsync(string name) => Task.FromResult(
            (IStorageFile?)WrapFileSystemInfo(GetFileCore(directoryInfo, name)));
    }
}
