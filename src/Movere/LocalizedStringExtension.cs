using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.MarkupExtensions.CompiledBindings;

using Movere.Models;
using Movere.Resources;

namespace Movere
{
    internal sealed class LocalizedStringExtension
    {
        private sealed class Converter : IMultiValueConverter
        {
            public static IMultiValueConverter Instance { get; } = new Converter();

            private Converter()
            {
            }

            public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
            {
                if (values.Any(x => x == AvaloniaProperty.UnsetValue))
                {
                    return null;
                }

                var key = ConvertKey(values[0]);
                return String.Format(key.GetString(culture), values.Skip(1).ToArray());
            }

            private static LocalizedString ConvertKey(object? key) =>
                key switch
                {
                    string str => new LocalizedString(Strings.ResourceManager, str),
                    LocalizedString loc => loc,
                    BindingNotification => String.Empty,
                    _ => throw new NotSupportedException("Key must be either string or LocalizedString!")
                };
        }

        public LocalizedStringExtension(object key)
        {
            Key = key;
            Args = [];
        }

        public LocalizedStringExtension(object key, object? arg0)
        {
            Key = key;
            Args = [arg0];
        }

        public LocalizedStringExtension(object key, object? arg0, object? arg1)
        {
            Key = key;
            Args = [arg0, arg1];
        }

        public LocalizedStringExtension(object key, object? arg0, object? arg1, object? arg2)
        {
            Key = key;
            Args = [arg0, arg1, arg2];
        }

        public object Key { get; }

        public object?[] Args { get; }

        public IBinding ProvideValue(IServiceProvider serviceProvider)
        {
            var key = (Key as IBinding) ?? CreateConstantBinding(Key);

            var args = Args
                .Select(x => (x as IBinding) ?? CreateConstantBinding(x))
                .ToArray();

            return new MultiBinding()
            {
                Bindings = [key, ..args],
                Converter = Converter.Instance
            };
        }

        private static CompiledBindingExtension CreateConstantBinding(object? value) =>
            new CompiledBindingExtension(
                new CompiledBindingPathBuilder()
                    .Build()
            )
            {
                Mode = BindingMode.OneTime,
                Source = value
            };
    }
}
