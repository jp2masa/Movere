using Avalonia;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace Movere.Behaviors
{
    internal sealed class FocusAction : AvaloniaObject, IAction
    {
        public static readonly StyledProperty<InputElement?> ControlProperty =
            AvaloniaProperty.Register<FocusAction, InputElement?>(nameof(Control));

        public InputElement? Control
        {
            get => GetValue(ControlProperty);
            set => SetValue(ControlProperty, value);
        }

        public object? Execute(object? sender, object? parameter)
        {
            if (!(Control is null))
            {
                Control.Focus();
            }

            return null;
        }
    }
}
