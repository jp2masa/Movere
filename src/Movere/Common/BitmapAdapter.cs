using System.Drawing.Imaging;
using System.IO;
using DrawingBitmap = System.Drawing.Bitmap;

using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Movere
{
    internal sealed class BitmapAdapter : Bitmap
    {
        public BitmapAdapter(DrawingBitmap bitmap)
            : base(GetPlaformImpl(bitmap))
        {
        }

        private static IBitmapImpl GetPlaformImpl(DrawingBitmap bitmap)
        {
            using var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);

            stream.Position = 0;

            var factory = AvaloniaLocator.Current.GetService<IPlatformRenderInterface>();

            return factory.LoadBitmap(stream);
        }
    }
}
