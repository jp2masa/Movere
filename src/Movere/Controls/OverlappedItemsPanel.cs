using System.Windows.Input;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Movere.Controls
{
    internal class OverlappedItemsPanel : Panel
    {
        public static readonly DirectProperty<OverlappedItemsPanel, ICommand?> TappedCommandProperty =
            AvaloniaProperty.RegisterDirect<OverlappedItemsPanel, ICommand?>(
                nameof(TappedCommand),
                p => p.TappedCommand,
                (p, c) => p.TappedCommand = c);

        public static readonly StyledProperty<object?> TappedCommandParameterProperty =
            AvaloniaProperty.Register<OverlappedItemsPanel, object?>(nameof(TappedCommandParameter));

        private ICommand? _tappedCommand;

        public OverlappedItemsPanel()
        {
            Tapped += OnTapped;
        }

        public ICommand? TappedCommand
        {
            get => _tappedCommand;
            set => SetAndRaise(TappedCommandProperty, ref _tappedCommand, value);
        }

        public object? TappedCommandParameter
        {
            get => GetValue(TappedCommandParameterProperty);
            set => SetValue(TappedCommandParameterProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var size = Size.Empty;

            foreach (var child in Children)
            {
                child.Measure(availableSize);

                if (child.DesiredSize.Width > size.Width)
                {
                    size = size.WithWidth(child.DesiredSize.Width);
                }

                if (child.DesiredSize.Height > size.Height)
                {
                    size = size.WithHeight(child.DesiredSize.Height);
                }
            }

            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (var child in Children)
            {
                if (child.IsVisible)
                {
                    child.Arrange(new Rect(default, finalSize));
                }
            }

            return finalSize;
        }

        protected void OnTapped(object sender, RoutedEventArgs e)
        {
            if (TappedCommand != null)
            {
                var parameter = TappedCommandParameter;

                if (TappedCommand.CanExecute(parameter))
                {
                    TappedCommand.Execute(parameter);
                }
            }
        }
    }
}
