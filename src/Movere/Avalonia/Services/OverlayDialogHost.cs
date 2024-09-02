using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;

using Movere.Services;
using Movere.ViewModels;
using Movere.Views;

namespace Movere.Avalonia.Services
{
    internal sealed class OverlayDialogHost(Application application, Visual target, IDataTemplate? dataTemplate = null)
        : DialogHostBase(application, dataTemplate)
    {
        private sealed class DialogView<TResult> : IDialogView<TResult>
        {
            private readonly OverlayLayer _layer;
            private readonly DialogOverlay _overlay;

            private readonly ISubject<TResult> _resultSubject = new Subject<TResult>();

            public DialogView(OverlayLayer layer, DialogOverlay overlay)
            {
                _layer = layer;
                _overlay = overlay;

                Result = _resultSubject.AsObservable();
            }

            public IObservable<TResult> Result { get; }

            public void Close(TResult result)
            {
                if (_overlay.OnClosing())
                {
                    _layer.Children.Remove(_overlay);

                    _resultSubject.OnNext(result);
                    _resultSubject.OnCompleted();
                }
            }
        }

        public override IObservable<TResult> ShowDialog<TContent, TResult>(
            Func<IDialogView<TResult>, IDialogWindowViewModel<TContent>> viewModelFactory
        )
        {
            var layer = OverlayLayer.GetOverlayLayer(target);

            if (layer is null)
            {
                return Observable.Create<TResult>(
                    observer =>
                    {
                        observer.OnError(new Exception());
                        return Disposable.Empty;
                    }
                );
            }

            var overlay = new DialogOverlay() { DataTemplates = { DataTemplate } };
            var view = new DialogView<TResult>(layer, overlay);

            overlay.DataContext = viewModelFactory(view);
            layer.Children.Add(overlay);

            return view.Result;
        }
    }
}
