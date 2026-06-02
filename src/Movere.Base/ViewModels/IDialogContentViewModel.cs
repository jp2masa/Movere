using System;

namespace Movere.ViewModels
{
    internal interface IDialogContentViewModel<out TResult>
    {
        IObservable<IObservable<TResult>> Result { get; }

        void Close();
    }
}
