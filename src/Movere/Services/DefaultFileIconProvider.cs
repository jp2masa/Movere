using System.Runtime.InteropServices;
using Icon = System.Drawing.Icon;

namespace Movere.Services
{
    internal sealed class DefaultFileIconProvider : IFileIconProvider
    {
        public static IFileIconProvider Instance { get; } = new DefaultFileIconProvider();

        public IFileIcon? GetFileIcon(string filePath)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return null;
            }

            return GetWindowsFileIcon(filePath);
        }

        private IFileIcon? GetWindowsFileIcon(string filePath) =>
            Icon.ExtractAssociatedIcon(filePath) is Icon icon
                ? new SystemDrawingIconFileIcon(icon)
                : null;
    }
}
