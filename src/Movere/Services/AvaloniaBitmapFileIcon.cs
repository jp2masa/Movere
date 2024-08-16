using System.IO;

using Avalonia.Media.Imaging;

namespace Movere.Services
{
    internal sealed class AvaloniaBitmapFileIcon : IFileIcon
    {
        public AvaloniaBitmapFileIcon(Bitmap bitmap)
        {
            Bitmap = bitmap;
        }

        public Bitmap Bitmap { get; }

        public void Save(Stream stream) =>
            Bitmap.Save(stream);
    }
}
