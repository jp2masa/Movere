using System;

using Avalonia;
using Avalonia.Controls;

namespace Movere.Controls
{
    internal sealed class BreadcrumbPanel : Panel
    {
        private int _n;

        protected override Size MeasureOverride(Size availableSize)
        {
            var width = 0.0;
            var height = 0.0;

            var childMeasureSize = new Size(Double.PositiveInfinity, availableSize.Height);

            for (_n = 0; _n < Children.Count; _n++)
            {
                var child = Children[Children.Count - _n - 1];

                child.Measure(childMeasureSize);

                if (width + child.DesiredSize.Width > availableSize.Width)
                {
                    break;
                }

                width += child.DesiredSize.Width;
                height = Math.Max(height, child.DesiredSize.Height);
            }

            for (var i = 0; i < Children.Count - _n; i++)
            {
                Children[i].Measure(default);
            }

            return new Size(width, height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var position = default(Point);

            for (var i = 0; i < Children.Count - _n; i++)
            {
                Children[i].Arrange(default);
            }

            for (var i = _n; i >= 1; i--)
            {
                var child = Children[Children.Count - i];

                child.Arrange(new Rect(position, new Size(child.DesiredSize.Width, finalSize.Height)));
                position = position.WithX(position.X + child.DesiredSize.Width);
            }

            return finalSize;
        }
    }
}
