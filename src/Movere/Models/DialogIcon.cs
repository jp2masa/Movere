using System;

using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Movere.Models
{
    public sealed class DialogIcon : IDialogIcon
    {
        public static IDialogIcon None { get; } = new EmptyDialogIcon();

        public static IDialogIcon Info { get; } = new DialogIcon("avares://Movere/Resources/Icons/Info.png");

        public static IDialogIcon Warning { get; } = new DialogIcon("avares://Movere/Resources/Icons/Warning.png");

        public static IDialogIcon Error { get; } = new DialogIcon("avares://Movere/Resources/Icons/Error.png");

        private readonly Uri _uri;
        
        public DialogIcon(string path)
        {
            _uri = new Uri(path);
        }

        public IBitmap? LoadIcon()
        {
            var assetLoader = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var iconStream = assetLoader.Open(_uri);

            if (iconStream == null)
            {
                return null;
            }

            return new Bitmap(iconStream);
        }

        private class EmptyDialogIcon : IDialogIcon
        {
            public IBitmap? LoadIcon() => null;
        }
    }
}
