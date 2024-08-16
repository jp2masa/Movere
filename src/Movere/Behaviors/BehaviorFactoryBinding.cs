using System;
using System.Globalization;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.MarkupExtensions.CompiledBindings;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;
using Avalonia.Xaml.Interactivity;

namespace Movere.Behaviors
{
    internal sealed class BehaviorFactoryBinding
    {
        private sealed class Converter(BehaviorFactoryBinding owner) : IValueConverter
        {
            public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
                owner.Content?.Build(value);

            public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
                throw new NotSupportedException();
        }

        [Content]
        public BehaviorCollectionTemplate? Content { get; set; }

        public IBinding ProvideValue(IServiceProvider serviceProvider) =>
            new CompiledBindingExtension(
                new CompiledBindingPathBuilder()
                    .Self()
                    .Build()
            )
            {
                Converter = new Converter(this)
            };
    }

    internal sealed class BehaviorCollectionTemplate : ITemplate<object?, BehaviorCollection?>
    {
        [Content]
        [TemplateContent]
        public object? Content { get; set; }

        public BehaviorCollection? Build(object? param)
        {
            var result = TemplateContent.Load(Content);

            if (result?.Result is not BehaviorFactoryControl control)
            {
                throw new InvalidOperationException("Invalid BehaviorFactoryBinding usage!");
            }

            var target = (AvaloniaObject)param!;
            control[~Control.DataContextProperty] = target[~Control.DataContextProperty];

            return control.Content;
        }
    }

    internal sealed class BehaviorFactoryControl : Control
    {
        [Content]
        public BehaviorCollection? Content { get; set; }
    }
}
