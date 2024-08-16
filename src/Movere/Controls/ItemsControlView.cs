using System.Collections.Generic;

using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;

namespace Movere.Controls
{
    public partial class ItemsControlView : StyledElement
    {
        private static readonly ITemplate<Panel?> DefaultPanelTemplate =
            new FuncTemplate<Panel?>(() => new VirtualizingStackPanel());

        public static readonly StyledProperty<ITemplate<Panel?>> PanelTemplateProperty =
            AvaloniaProperty.Register<ItemsControlView, ITemplate<Panel?>>(nameof(PanelTemplate), DefaultPanelTemplate);

        public static readonly AttachedProperty<ItemsControlView> ItemsViewProperty =
            AvaloniaProperty.RegisterAttached<ItemsControlView, ItemsControl, ItemsControlView>("ItemsView");

        static ItemsControlView()
        {
            ItemsViewProperty.Changed.AddClassHandler<ItemsControl>(UpdateItemsView);
        }

        public ItemsControlView()
        {
            DataTemplates = new AvaloniaList<IDataTemplate>();
        }

        public IList<IDataTemplate> DataTemplates { get; }

        private static void UpdateItemsView(ItemsControl sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is null)
            {
                sender.SetCurrentValue(ItemsControl.ItemsPanelProperty, sender.GetBaseValue(ItemsControl.ItemsPanelProperty));
                sender.DataTemplates.Clear();

                return;
            }

            var view = (ItemsControlView)e.NewValue;

            sender.SetCurrentValue(ItemsControl.ItemsPanelProperty, view.PanelTemplate);

            sender.DataTemplates.Clear();
            sender.DataTemplates.AddRange(view.DataTemplates);

            var template = sender.GetValue(TemplatedControl.TemplateProperty);

            sender.SetCurrentValue(TemplatedControl.TemplateProperty, null);
            sender.ApplyTemplate();

            sender.SetCurrentValue(TemplatedControl.TemplateProperty, template);
            sender.ApplyTemplate();
        }
    }
}
