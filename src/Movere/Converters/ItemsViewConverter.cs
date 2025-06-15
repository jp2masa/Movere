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
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is ItemsView view && targetType == typeof(ItemsControlView) && parameter is ResourceDictionary resources)
            {
                if (resources.TryGetResource(view, null, out var result))
                {
                    return result;
                }

                throw new InvalidOperationException("Item view not found!");
            }

            throw new NotSupportedException();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
