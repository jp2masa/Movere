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

            return GetWindowsFileIcon(filePath);
        }

        private IBitmap? GetWindowsFileIcon(string filePath) =>
            Icon.ExtractAssociatedIcon(filePath) is Icon icon
                ? new BitmapAdapter(icon.ToBitmap())
                : null;
    }
}
