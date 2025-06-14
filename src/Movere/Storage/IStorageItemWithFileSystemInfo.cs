// https://github.com/AvaloniaUI/Avalonia/blob/13413579b5677cd8740c41b466a1e11c2c8c3e2e/src/Avalonia.Base/Platform/Storage/IStorageBookmarkItem.cs#L7-L10

using System.IO;

using Avalonia.Platform.Storage;

namespace Movere.Storage
{
    internal interface IStorageItemWithFileSystemInfo : IStorageItem
    {
        FileSystemInfo FileSystemInfo { get; }
    }
}
