using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Input.Platform;

namespace Movere.Services
{
    internal sealed class ClipboardService : IClipboardService
    {
        private readonly IClipboard _clipboard;

        public ClipboardService()
        {
            _clipboard = AvaloniaLocator.Current.GetService<IClipboard>();
        }

        public Task ClearAsync() => _clipboard.ClearAsync();

        public Task<string> GetTextAsync() => _clipboard.GetTextAsync();

        public Task SetTextAsync(string text) => _clipboard.SetTextAsync(text);

        public Task<IReadOnlyCollection<string>> GetFilesAsync() =>
            Task.FromResult<IReadOnlyCollection<string>>(Array.Empty<string>());

        public Task SetFilesAsync(IReadOnlyCollection<string> files) => Task.CompletedTask;
    }
}
