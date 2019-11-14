using System;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;

using Avalonia.Media.Imaging;

namespace Movere.ViewModels
{
    internal sealed class PrintPreviewPageViewModel
    {
        private readonly PreviewPageInfo _pageInfo;

        private readonly Lazy<Bitmap> _image;

        public PrintPreviewPageViewModel(PreviewPageInfo pageInfo)
        {
            _pageInfo = pageInfo;

            _image = new Lazy<Bitmap>(LoadImage);
        }

        public Bitmap Image => _image.Value;

        public int Width => _pageInfo.PhysicalSize.Width;

        public int Height => _pageInfo.PhysicalSize.Height;

        private Bitmap LoadImage()
        {
            var image = _pageInfo.Image;

            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Bmp);

            stream.Position = 0;

            return new Bitmap(stream);
        }
    }
}
