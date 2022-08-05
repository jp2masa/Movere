using System.IO;

using Avalonia.Media.Imaging;

namespace Movere.Services
{
    internal sealed class AvaloniaBitmapFileIcon : IFileIcon
    {
        public AvaloniaBitmapFileIcon(IBitmap bitmap)
        {
            Bitmap = bitmap;
        }

        public IBitmap Bitmap { get; }

        public void Save(Stream stream) => Bitmap.Save(stream);
    }
}
