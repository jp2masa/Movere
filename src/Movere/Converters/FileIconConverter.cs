using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MemoryStream = System.IO.MemoryStream;

using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

using Movere.Models;
using Movere.Services;

namespace Movere.Converters
{
    internal sealed class FileIconConverter : IMultiValueConverter
    {
        private readonly Lazy<Bitmap> _defaultFileIcon =
            new Lazy<Bitmap>(LoadDefaultFileIcon);

        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Any(v => v == AvaloniaProperty.UnsetValue))
            {
                return BindingOperations.DoNothing;
            }

            if (values.Count != 2
                || !targetType.IsAssignableFrom(typeof(IImage)))
            {
                throw new NotSupportedException();
            }

            if (values[0] is File file
                && values[1] is IFileIconProvider fileIconProvider)
            {
                return fileIconProvider.GetFileIcon(file.FullPath) switch
                {
                    AvaloniaBitmapFileIcon avalonia => avalonia.Bitmap,
                    IFileIcon icon => ConvertIconToBitmap(icon),
                    null => _defaultFileIcon.Value
                };
            }

            return _defaultFileIcon.Value;
        }

        private static Bitmap LoadDefaultFileIcon()
        {
            var assetLoader = AvaloniaLocator.Current.GetRequiredService<IAssetLoader>();
            var stream = assetLoader.Open(new Uri("avares://Movere/Resources/Icons/File.png"));

            return new Bitmap(stream);
        }

        private static Bitmap ConvertIconToBitmap(IFileIcon icon)
        {
            using var stream = new MemoryStream();
            icon.Save(stream);

            stream.Position = 0;

            return new Bitmap(stream);
        }
    }
}
