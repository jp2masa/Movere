using Icon = System.Drawing.Icon;

using Avalonia.Media.Imaging;

namespace Movere.Services
{

    internal sealed class DefaultFileIconProvider : IFileIconProvider
    {
        internal static IFileIconProvider Instance { get; } = new DefaultFileIconProvider();

        public IBitmap? GetFileIcon(string filePath)
        {
            var icon = Icon.ExtractAssociatedIcon(filePath);

            if (icon == null)
            {
                return null;
            }

            var bitmap = icon.ToBitmap();
            return new BitmapAdapter(bitmap);
        }
    }
}
