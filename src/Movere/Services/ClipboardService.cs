using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Input.Platform;
using Avalonia.Platform.Storage;

using Movere.Storage;

namespace Movere.Services
{
    public sealed class ClipboardService : IClipboardService
    {
        private static readonly IClipboard s_clipboard = AvaloniaLocator.Current.GetRequiredService<IClipboard>();

        public Task ClearAsync() =>
            s_clipboard.ClearAsync();

        public Task<string?> GetTextAsync() =>
            s_clipboard.TryGetTextAsync();

        public Task SetTextAsync(string? text) =>
            s_clipboard.SetTextAsync(text);

        public async Task<IReadOnlyCollection<string>> GetFilesAsync() =>
            await s_clipboard.TryGetFilesAsync() is { } files
                ? files.Select(x => x.Path.LocalPath).ToImmutableArray()
                : Array.Empty<string>();

        public Task SetFilesAsync(IReadOnlyCollection<string> files) =>
            s_clipboard.SetFilesAsync(
                files
                    .Select(
                        x =>
                            Directory.Exists(x)
                                ? (IStorageItem)new BclStorageFolder(new DirectoryInfo(x))
                                : new BclStorageFile(new FileInfo(x))
                    )
            );
    }
}
