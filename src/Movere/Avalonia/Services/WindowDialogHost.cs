using System;
using System.Reactive.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

using Movere.Services;
using Movere.ViewModels;
using Movere.Views;

namespace Movere.Avalonia.Services
{
    internal sealed class WindowDialogHost(Application application, Window owner, IDataTemplate? dataTemplate = null)
        : DialogHostBase(application, dataTemplate)
    {
        private sealed class DialogView<TResult>(Window window) : IDialogView<TResult>
        {
            public void Close(TResult result) =>
                window.Close(result);
        }

        public override IObservable<TResult> ShowDialog<TContent, TResult>(
            Func<IDialogView<TResult>, IDialogWindowViewModel<TContent>> viewModelFactory
        )
        {
            var window = new DialogWindow() { DataTemplates = { DataTemplate } };
            window.DataContext = viewModelFactory(new DialogView<TResult>(window));

            return window.ShowDialog<TResult>(owner).ToObservable();
        }
    }
}
