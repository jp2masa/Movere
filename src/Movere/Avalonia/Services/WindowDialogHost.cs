using System;
using System.Reactive.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

using Movere.Views;

namespace Movere.Avalonia.Services
{
    public sealed class WindowDialogHost(Application application, Window owner, IDataTemplate? dataTemplate = null)
        : DialogHostBase(application, dataTemplate)
    {
        private sealed class AvaloniaDialogView<TResult>(Window owner, Window window, DialogHostBase host)
            : AvaloniaDialogViewBase<TResult>
        {
            public override Control Root =>
                window;

            public override DialogHostBase Host =>
                host;

            public override IObservable<TResult> Show() =>
                window
                    .ShowDialog<TResult>(owner)
                    .ToObservable();

            public override void Close(TResult result) =>
                window.Close(result);
        }

        public Window Owner =>
            owner;

        protected override AvaloniaDialogViewBase<TResult>? CreateAvaloniaDialogView<TResult>() =>
            new DialogWindow() is { } window
                && new WindowDialogHost(Application, window, DataTemplate) is { } host
                ? new AvaloniaDialogView<TResult>(owner, window, host)
                : null;
    }
}
