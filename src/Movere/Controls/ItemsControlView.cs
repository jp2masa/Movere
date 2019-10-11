using System.Collections.Generic;

using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Movere.Controls
{
    public class ItemsControlView : AvaloniaObject
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
            var view = (ItemsControlView)e.NewValue;

            sender.ItemsPanel = view.PanelTemplate;

            sender.DataTemplates.Clear();
            sender.DataTemplates.AddRange(view.DataTemplates);

            var template = sender.Template;

            sender.Template = null;
            sender.ApplyTemplate();

            sender.Template = template;
            sender.ApplyTemplate();
        }
    }
}
