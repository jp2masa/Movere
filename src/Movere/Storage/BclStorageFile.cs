// https://github.com/AvaloniaUI/Avalonia/blob/13413579b5677cd8740c41b466a1e11c2c8c3e2e/src/Avalonia.Base/Platform/Storage/FileIO/BclStorageFile.cs

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
