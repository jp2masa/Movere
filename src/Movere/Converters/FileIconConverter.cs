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
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(v => v == AvaloniaProperty.UnsetValue))
            {
                return BindingOperations.DoNothing;
            }

            if (values.Count == 2 && values[0] is File file && values[1] is IFileIconProvider fileIconProvider && targetType.IsAssignableFrom(typeof(Bitmap)))
            {
                var icon = fileIconProvider.GetFileIcon(file.FullPath);

                if (icon == null)
                {
                    var assetLoader = AvaloniaLocator.Current.GetService<IAssetLoader>();
                    var stream = assetLoader.Open(new Uri("avares://Movere/Resources/Icons/File.png"));

                    icon = new Bitmap(stream);
                }

                return icon;
            }

            throw new NotSupportedException();
        }
    }
}
