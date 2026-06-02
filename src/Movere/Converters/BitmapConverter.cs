using System;
using System.Globalization;

using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

using Movere.Models;

namespace Movere.Converters
{
    internal sealed class BitmapConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            value is IBitmap bitmap && targetType.IsAssignableFrom(typeof(Bitmap))
                ? CreateBitmap(bitmap)
                : throw new NotSupportedException();

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            throw new NotSupportedException();

        private static Bitmap CreateBitmap(IBitmap bitmap)
        {
            using var stream = bitmap.Open();
            return new Bitmap(stream);
        }
    }
}
