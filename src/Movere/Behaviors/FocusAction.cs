using Avalonia;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace Movere.Behaviors
{
    internal sealed partial class FocusAction : AvaloniaObject, IAction
    {
        public static readonly StyledProperty<InputElement?> ControlProperty =
            AvaloniaProperty.Register<FocusAction, InputElement?>(nameof(Control));

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
