using System;
using System.Globalization;

using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace Movere.Converters
{
    internal sealed class AllowMultipleSelectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b && targetType == typeof(SelectionMode))
            {
                return b ? SelectionMode.Multiple : SelectionMode.Single;
            }

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
