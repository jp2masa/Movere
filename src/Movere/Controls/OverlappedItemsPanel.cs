using System;

using Avalonia;
using Avalonia.Controls;

namespace Movere.Controls
{
    internal sealed class OverlappedItemsPanel : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            var width = 0.0;
            var height = 0.0;

            foreach (var child in Children)
            {
                child.Measure(availableSize);

                width = Math.Max(width, child.DesiredSize.Width);
                height = Math.Max(height, child.DesiredSize.Height);
            }

            return new Size(width, height);
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
