using System.Collections.Generic;

using Avalonia;
using Avalonia.Controls;

namespace Movere.Controls
{
    internal sealed class BreadcrumbPanel : Panel
    {
        private readonly Stack<IControl> _children = new Stack<IControl>();

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_children.Count != 0)
            {
                _children.Clear();
            }

            var size = Size.Empty;
            var width = 0.0;

            int i;

            for (i = Children.Count - 1; i >= 0; i--)
            {
                var child = Children[i];

                if (!child.IsVisible)
                {
                    break;
                }

                child.Measure(availableSize);

                if (width + child.DesiredSize.Width > availableSize.Width)
                {
                    break;
                }

                width += child.DesiredSize.Width;

                if (child.DesiredSize.Height > size.Height)
                {
                    size = size.WithHeight(child.DesiredSize.Height);
                }

                _children.Push(child);
            }

            for (; i >= 0; i--)
            {
                var child = Children[i];
                child.Measure(availableSize);
            }

            return size.WithWidth(width);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var position = default(Point);

            while (_children.TryPop(out var child))
            {
                child.Arrange(new Rect(position, child.DesiredSize));
                position = position.WithX(position.X + child.DesiredSize.Width);
            }

            return finalSize;
        }
    }
}
