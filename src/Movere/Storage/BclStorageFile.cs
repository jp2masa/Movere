// https://github.com/AvaloniaUI/Avalonia/blob/82d64089e15dca3712dc87dce757a29ccef2a04e/src/Avalonia.Base/Platform/Storage/FileIO/BclStorageFile.cs

using System.IO;
using System.Threading.Tasks;

using Avalonia.Platform.Storage;

namespace Movere.Storage
{
    internal sealed class BclStorageFile(FileInfo fileInfo) : BclStorageItem(fileInfo), IStorageBookmarkFile
    {
        public Task<Stream> OpenReadAsync() => Task.FromResult<Stream>(OpenReadCore(fileInfo));
        public Task<Stream> OpenWriteAsync() => Task.FromResult<Stream>(OpenWriteCore(fileInfo));
    }
}
