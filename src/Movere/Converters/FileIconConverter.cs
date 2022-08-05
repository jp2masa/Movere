using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
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

            if (values.Count == 2 && values[0] is File file && values[1] is IFileIconProvider fileIconProvider && targetType.IsAssignableFrom(typeof(Bitmap)))
            {
                return fileIconProvider.GetFileIcon(file.FullPath)
                    ?? _defaultFileIcon.Value;
            }

            throw new NotSupportedException();
        }

        private static Bitmap LoadDefaultFileIcon()
        {

            var assetLoader = AvaloniaLocator.Current.GetRequiredService<IAssetLoader>();
            var stream = assetLoader.Open(new Uri("avares://Movere/Resources/Icons/File.png"));

            return new Bitmap(stream);
        }
    }
}
