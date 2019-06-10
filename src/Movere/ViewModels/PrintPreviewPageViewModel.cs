using System;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Avalonia.Media.Imaging;

namespace Movere.ViewModels
{
    internal sealed class PrintPreviewPageViewModel
    {
        private readonly PreviewPageInfo _pageInfo;

        private Lazy<Task<Bitmap>> _image;

        public PrintPreviewPageViewModel(PreviewPageInfo pageInfo)
        {
            _pageInfo = pageInfo;

            _image = new Lazy<Task<Bitmap>>(LoadImageAsync);
        }

        public Task<Bitmap> Image => _image.Value;

        public int Width => _pageInfo.PhysicalSize.Width;

        public int Height => _pageInfo.PhysicalSize.Height;

        private Task<Bitmap> LoadImageAsync() =>
            Task.Run(
                () =>
                {
                    var image = _pageInfo.Image;

                    var stream = new MemoryStream();
                    image.Save(stream, ImageFormat.Bmp);

                    stream.Position = 0;
                    return new Bitmap(stream);
                });
    }
}
