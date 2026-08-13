// https://github.com/AvaloniaUI/Avalonia/blob/e33eaed9c106846b200680751022385d9cc5dc6f/src/Avalonia.Base/Platform/Storage/IStorageBookmarkItem.cs#L7-L10

using System.IO;

using Avalonia.Platform.Storage;

namespace Movere.Storage
{
    internal interface IStorageItemWithFileSystemInfo : IStorageItem
    {
        FileSystemInfo FileSystemInfo { get; }
    }
}
