using System;
using System.IO;

using Avalonia.Platform;

namespace Movere.Models
{
    public sealed class AvaloniaDialogIcon : IDialogIcon
    {
        private sealed class AvaloniaBitmap(Uri uri) : IBitmap
        {
            private readonly Uri _uri = uri;

            public Stream Open() =>
                AssetLoader.Open(_uri);
        }

        public static IDialogIcon Null { get; } = new AvaloniaDialogIcon(null);

        public static IDialogIcon? Info { get; } = TryCreate("avares://Movere/Resources/Icons/Info.png");

        public static IDialogIcon? Warning { get; } = TryCreate("avares://Movere/Resources/Icons/Warning.png");

        public static IDialogIcon? Error { get; } = TryCreate("avares://Movere/Resources/Icons/Error.png");

        private readonly AvaloniaBitmap? _bitmap;

        private AvaloniaDialogIcon(Uri? uri)
        {
            if (uri is null)
            {
                return;
            }
            
            _bitmap = new AvaloniaBitmap(uri);
        }

        public IBitmap? LoadIcon() =>
            _bitmap;

        public static AvaloniaDialogIcon? TryCreate(string path)
        {
            var uri = new Uri(path);

            return AssetLoader.Exists(uri)
                ? new AvaloniaDialogIcon(uri)
                : null;
        }
    }
}
