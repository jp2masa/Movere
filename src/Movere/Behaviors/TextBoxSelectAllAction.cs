using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Movere.Behaviors
{
    internal sealed class TextBoxSelectAllAction : AvaloniaObject, IAction
    {
        public static readonly StyledProperty<TextBox?> TextBoxProperty =
            AvaloniaProperty.Register<FocusAction, TextBox?>(nameof(TextBox));

        public TextBox? TextBox
        {
            get => GetValue(TextBoxProperty);
            set => SetValue(TextBoxProperty, value);
        }

        public object? Execute(object? sender, object? parameter)
        {
            if (!(TextBox is null))
            {
                TextBox.SelectAll();
            }

            return null;
        }
    }
}
