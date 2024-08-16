using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Input;
using Avalonia.Input.Platform;

namespace Movere.Services
{
    public sealed class ClipboardService : IClipboardService
    {
        private static readonly IClipboard s_clipboard = AvaloniaLocator.Current.GetRequiredService<IClipboard>();

        public Task ClearAsync() => s_clipboard.ClearAsync();

        public Task<string?> GetTextAsync() => s_clipboard.GetTextAsync();

        public Task SetTextAsync(string? text) => s_clipboard.SetTextAsync(text);

        public async Task<IReadOnlyCollection<string>> GetFilesAsync()
        {
            var result = await s_clipboard.GetDataAsync(DataFormats.FileNames);

            if (result is IReadOnlyCollection<string> files)
            {
                return files;
            }

            return Array.Empty<string>();
        }

        public Task SetFilesAsync(IReadOnlyCollection<string> files)
        {
            var data = new DataObject();
            data.Set(DataFormats.FileNames, files);

            return s_clipboard.SetDataObjectAsync(data);
        }
    }
}
