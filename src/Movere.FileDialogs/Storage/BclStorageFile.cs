// https://github.com/AvaloniaUI/Avalonia/blob/e33eaed9c106846b200680751022385d9cc5dc6f/src/Avalonia.Base/Platform/Storage/FileIO/BclStorageFile.cs

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
