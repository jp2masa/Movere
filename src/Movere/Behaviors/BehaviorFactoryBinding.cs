using System;
using System.Reactive.Subjects;
using Avalonia;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;
using Avalonia.Xaml.Interactivity;

namespace Movere.Behaviors
{
    internal sealed class BehaviorFactoryBinding : IBinding
    {
        [Content]
        public BehaviorCollectionTemplate? Content { get; set; }

        public InstancedBinding? Initiate(
            AvaloniaObject target,
            AvaloniaProperty? targetProperty,
            object? anchor = null,
            bool enableDataValidation = false) =>
            InstancedBinding.OneWay(
                new BehaviorSubject<object?>(
                    Content?.Build(null)
                        ?? throw new InvalidOperationException("Invalid BehaviorFactoryBinding usage!")
                ),
                BindingPriority.Style
            );
    }

    internal sealed class BehaviorCollectionTemplate : ITemplate<object?, BehaviorCollection?>
    {
        [Content]
        [TemplateContent(TemplateResultType = typeof(BehaviorCollection))]
        public object? Content { get; set; }

        public BehaviorCollection? Build(object? param) =>
            TemplateContent.Load<BehaviorCollection>(Content).Result;
    }
}
