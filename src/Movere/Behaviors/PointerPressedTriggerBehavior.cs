using Avalonia;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace Movere.Behaviors
{
    internal sealed partial class PointerPressedTriggerBehavior : Trigger
    {
        public static readonly DirectProperty<PointerPressedTriggerBehavior, PointerUpdateKind> PointerUpdateKindProperty =
            AvaloniaProperty.RegisterDirect<PointerPressedTriggerBehavior, PointerUpdateKind>(
                nameof(PointerUpdateKind),
                o => o.PointerUpdateKind,
                (o, v) => o.PointerUpdateKind = v);

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is not InputElement input)
            {
                return;
            }

            input.PointerPressed += OnPointerPressed;
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject is not InputElement input)
            {
                return;
            }

            input.PointerPressed -= OnPointerPressed;

            base.OnDetaching();
        }

        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var point = e.GetCurrentPoint((InputElement)AssociatedObject!);

            if (point.Properties.PointerUpdateKind == PointerUpdateKind)
            {
                Interaction.ExecuteActions(sender, Actions, null);
            }
        }
    }
}
