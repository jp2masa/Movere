using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Movere.Behaviors
{
    internal sealed partial class TextBoxSelectAllAction : AvaloniaObject, IAction
    {
        public static readonly StyledProperty<TextBox?> TextBoxProperty =
            AvaloniaProperty.Register<FocusAction, TextBox?>(nameof(TextBox));

        public object? Execute(object? sender, object? parameter)
        {
            TextBox?.SelectAll();
            return null;
        }
    }
}
