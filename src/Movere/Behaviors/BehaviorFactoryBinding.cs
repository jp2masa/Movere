using System;
using System.Reactive.Subjects;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Metadata;
using Avalonia.Xaml.Interactivity;

namespace Movere.Behaviors
{
    internal sealed class BehaviorFactoryBinding : IBinding
    {
        [Content]
        public BehaviorCollectionTemplate? Content { get; set; }

        public InstancedBinding? Initiate(
            IAvaloniaObject target,
            AvaloniaProperty? targetProperty,
            object? anchor = null,
            bool enableDataValidation = false) =>
            new InstancedBinding(
                new BehaviorSubject<object?>(
                    Content?.Build(target)
                    ?? throw new InvalidOperationException("Invalid BehaviorFactoryBinding usage!")),
                BindingMode.OneWay,
                BindingPriority.Style);
    }

    internal sealed class BehaviorCollectionTemplate : ITemplate<object?, BehaviorCollection?>
    {
        [Content]
        [TemplateContent]
        public object? Content { get; set; }

        public BehaviorCollection? Build(object? param)
        {
            var func = (Func<IServiceProvider?, object?>)Content!;
            var result = (ControlTemplateResult)func.Invoke(null)!;
            var control = (BehaviorFactoryControl)result.Control;

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
