using System;
using System.Globalization;

using Avalonia.Data.Converters;

using Movere.Models;

namespace Movere.Converters
{
    internal sealed class DialogResultIsCancelConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DialogResult result && targetType == typeof(bool))
            {
                return result == DialogResult.Cancel;
            }

            throw new NotSupportedException();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
