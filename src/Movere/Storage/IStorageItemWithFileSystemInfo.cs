// https://github.com/AvaloniaUI/Avalonia/blob/82d64089e15dca3712dc87dce757a29ccef2a04e/src/Avalonia.Base/Platform/Storage/IStorageBookmarkItem.cs#L7-L10

using System.IO;

using Avalonia.Platform.Storage;

namespace Movere.Storage
{
    internal interface IStorageItemWithFileSystemInfo : IStorageItem
    {
        FileSystemInfo FileSystemInfo { get; }
    }
}
