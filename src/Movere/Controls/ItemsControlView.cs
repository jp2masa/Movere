using System.Collections.Generic;

using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;

namespace Movere.Controls
{
    public class ItemsControlView : StyledElement
    {
        private static readonly ITemplate<IPanel> DefaultPanelTemplate =
            new FuncTemplate<IPanel>(() => new VirtualizingStackPanel());

        public static readonly StyledProperty<ITemplate<IPanel>> PanelTemplateProperty =
            AvaloniaProperty.Register<ItemsControlView, ITemplate<IPanel>>(nameof(PanelTemplate), DefaultPanelTemplate);

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

        public ITemplate<IPanel> PanelTemplate
        {
            get => GetValue(PanelTemplateProperty);
            set => SetValue(PanelTemplateProperty, value);
        }

        public IList<IDataTemplate> DataTemplates { get; }

        public static ItemsControlView GetItemsView(ItemsControl control) =>
            control.GetValue(ItemsViewProperty);

        public static void SetItemsView(ItemsControl control, ItemsControlView view) =>
            control.SetValue(ItemsViewProperty, view);

        private static void UpdateItemsView(ItemsControl sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is null)
            {
                sender.ItemsPanel = ItemsControl.ItemsPanelProperty.GetDefaultValue(sender.GetType());
                sender.DataTemplates.Clear();

                return;
            }

            var view = (ItemsControlView)e.NewValue;

            sender.ItemsPanel = view.PanelTemplate;

            sender.DataTemplates.Clear();
            sender.DataTemplates.AddRange(view.DataTemplates);

            var template = sender.GetBaseValue(TemplatedControl.TemplateProperty, BindingPriority.LocalValue);

            sender.SetValue(TemplatedControl.TemplateProperty, null);
            sender.ApplyTemplate();

            if (template.HasValue)
            {
                sender.SetValue(TemplatedControl.TemplateProperty, template.Value);
            }
            else
            {
                sender.ClearValue(TemplatedControl.TemplateProperty);
            }

            sender.ApplyTemplate();
        }
    }
}
