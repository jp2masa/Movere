using System;
using System.Globalization;

using Avalonia.Controls;
using Avalonia.Data.Converters;

using Movere.Controls;
using Movere.Models;

namespace Movere.Converters
{
    internal sealed class ItemsViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ItemsView view && targetType == typeof(ItemsControlView) && parameter is ResourceDictionary resources)
            {
                if (resources.TryGetResource(view, out var result))
                {
                    return result;
                }

#pragma warning disable CA1303 // Do not pass literals as localized parameters
                throw new InvalidOperationException("Item view not found!");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            }

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
