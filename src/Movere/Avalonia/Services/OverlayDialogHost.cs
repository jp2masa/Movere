using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;

using Movere.Views;

namespace Movere.Avalonia.Services
{
    public sealed class OverlayDialogHost(Application application, Visual target, IDataTemplate? dataTemplate = null)
        : DialogHostBase(application, dataTemplate)
    {
        private sealed class AvaloniaDialogView<TResult>(OverlayLayer layer, DialogOverlay overlay, DialogHostBase host)
            : AvaloniaDialogViewBase<TResult>
        {
            private readonly ISubject<TResult> _resultSubject = new Subject<TResult>();

            public OverlayLayer Layer =>
                layer;

            public override Control Root =>
                overlay;

            public override DialogHostBase Host =>
                host;

            public override IObservable<TResult> Show()
            {
                var disposable = (Layer.Parent as VisualLayerManager)?.Child is { } child
                    ? new VisibilityDisposable(child)
                    : null;

                Layer.Children.Add(overlay);

                return _resultSubject
                    .Do(_ => disposable?.Dispose());
            }

            public override void Close(TResult result)
            {
                if (overlay.OnClosing())
                {
                    Layer.Children.Remove(overlay);

                    _resultSubject.OnNext(result);
                    _resultSubject.OnCompleted();
                }
            }
        }

        private sealed class VisibilityDisposable : IDisposable
        {
            private readonly Control _control;
            private readonly bool _wasEnabled;

            public VisibilityDisposable(Control control)
            {
                _control = control;
                _wasEnabled = _control.GetValue(InputElement.IsEnabledProperty);

                _control.SetCurrentValue(InputElement.IsEnabledProperty, false);
            }

            public void Dispose()
            {
                _control.SetCurrentValue(InputElement.IsEnabledProperty, _wasEnabled);
            }
        }

        public Visual Target =>
            target;

        protected override AvaloniaDialogViewBase<TResult>? CreateAvaloniaDialogView<TResult>() =>
            OverlayLayer.GetOverlayLayer(target) is { } layer
                && new DialogOverlay() is { } overlay
                && new OverlayDialogHost(Application, overlay, DataTemplate) is { } host
                ? new AvaloniaDialogView<TResult>(layer, overlay, host)
                : null;
    }
}
