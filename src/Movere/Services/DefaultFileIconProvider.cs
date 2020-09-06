using System.Runtime.InteropServices;
using Icon = System.Drawing.Icon;

using Avalonia.Media.Imaging;

namespace Movere.Services
{

    internal sealed class DefaultFileIconProvider : IFileIconProvider
    {
        internal static IFileIconProvider Instance { get; } = new DefaultFileIconProvider();

        public IBitmap? GetFileIcon(string filePath)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return null;
            }

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
