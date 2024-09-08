using System;
using System.Globalization;
using MemoryStream = System.IO.MemoryStream;

using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

using Movere.Models;
using Movere.Services;
using Movere.ViewModels;

namespace Movere.Converters
{
    internal sealed class FileIconConverter : IValueConverter
    {
        private readonly Lazy<Bitmap> _defaultFileIcon =
            new Lazy<Bitmap>(LoadDefaultFileIcon);

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return AvaloniaProperty.UnsetValue;
            }

            if (value is not FileSystemEntryViewModel vm
                || !targetType.IsAssignableFrom(typeof(IImage)))
            {
                throw new NotSupportedException();
            }

            if (vm.Entry is Folder)
            {
                return null;
            }

            if (vm.Entry is not File)
            {
                throw new NotSupportedException();
            }

            return vm.Icon switch
            {
                AvaloniaBitmapFileIcon avalonia => avalonia.Bitmap,
                { } icon => ConvertIconToBitmap(icon),
                null => _defaultFileIcon.Value
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            throw new NotSupportedException();

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
