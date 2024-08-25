using System;
using System.IO;

using Avalonia.Platform;

namespace Movere.Models
{
    public sealed class AvaloniaDialogIcon : IDialogIcon
    {
        private sealed class AvaloniaBitmap : IBitmap
        {
            private readonly Uri _uri;

            public AvaloniaBitmap(Uri uri)
            {
                _uri = uri;
            }

            public Stream Open() =>
                AssetLoader.Open(_uri);
        }

        public static IDialogIcon Info { get; } = new AvaloniaDialogIcon("avares://Movere/Resources/Icons/Info.png");

        public static IDialogIcon Warning { get; } = new AvaloniaDialogIcon("avares://Movere/Resources/Icons/Warning.png");

        public static IDialogIcon Error { get; } = new AvaloniaDialogIcon("avares://Movere/Resources/Icons/Error.png");

        private readonly AvaloniaBitmap? _bitmap;

        public AvaloniaDialogIcon(string path)
        {
            var uri = new Uri(path);

            _bitmap = AssetLoader.Exists(uri)
                ? new AvaloniaBitmap(uri)
                : null;
        }

        public IBitmap? LoadIcon() =>
            _bitmap;
    }
}
