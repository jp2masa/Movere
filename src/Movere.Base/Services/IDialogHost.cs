using System;

using Movere.ViewModels;

namespace Movere.Services
{
    public interface IDialogHost : IAsyncDisposable
    {
        IObservable<TResult> ShowDialog<TContent, TResult>(
            Func<IDialogView<TResult>, IDialogWindowViewModel<TContent>> viewModelFactory
        );
    }
}
