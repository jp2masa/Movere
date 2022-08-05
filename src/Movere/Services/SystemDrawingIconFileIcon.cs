using System.Drawing;
using System.IO;

namespace Movere.Services
{
    internal sealed class SystemDrawingIconFileIcon : IFileIcon
    {
        public SystemDrawingIconFileIcon(Icon icon)
        {
            Icon = icon;
        }

        public Icon Icon { get; }

        public void Save(Stream stream) => Icon.Save(stream);
    }
}
