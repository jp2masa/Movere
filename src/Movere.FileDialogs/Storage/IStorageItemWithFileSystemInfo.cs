// https://github.com/AvaloniaUI/Avalonia/blob/6e04c167f0aead96a7489f88779d596d6d3766c8/src/Avalonia.Base/Platform/Storage/IStorageBookmarkItem.cs#L7-L10

using System.IO;

using Avalonia.Platform.Storage;

namespace Movere.Storage
{
    internal interface IStorageItemWithFileSystemInfo : IStorageItem
    {
        FileSystemInfo FileSystemInfo { get; }
    }
}
