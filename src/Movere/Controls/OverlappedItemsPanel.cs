using Avalonia;
using Avalonia.Controls;

namespace Movere.Controls
{
    internal class OverlappedItemsPanel : Panel
    {
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
    }
}
