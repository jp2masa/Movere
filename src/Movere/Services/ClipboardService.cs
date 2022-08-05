using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Input;
using Avalonia.Input.Platform;

namespace Movere.Services
{
    internal sealed class ClipboardService : IClipboardService
    {
        internal static IClipboardService Instance { get; } =
            new ClipboardService(AvaloniaLocator.Current.GetRequiredService<IClipboard>());

        private readonly IClipboard _clipboard;

        public ClipboardService(IClipboard clipboard)
        {
            _clipboard = clipboard;
        }

        public Task ClearAsync() => _clipboard.ClearAsync();

        public Task<string> GetTextAsync() => _clipboard.GetTextAsync();

        public Task SetTextAsync(string text) => _clipboard.SetTextAsync(text);

        public async Task<IReadOnlyCollection<string>> GetFilesAsync()
        {
            var result = await _clipboard.GetDataAsync(DataFormats.FileNames);

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

            return _clipboard.SetDataObjectAsync(data);
        }
    }
}
