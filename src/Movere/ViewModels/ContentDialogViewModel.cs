using System;
using System.Collections;
using System.Reactive.Linq;
using System.Windows.Input;

using ReactiveUI;

namespace Movere.ViewModels
{
    internal interface IContentDialogViewModel
    {
        object? Content { get; }

        IEnumerable Actions { get; }

        ICommand CloseCommand { get; }
    }

    internal static class ContentDialogViewModel
    {
        public static ContentDialogViewModel<TContent, TResult> Create<TContent, TResult>(
            TContent content,
            DialogActionSet<TContent, TResult> actions
        ) =>
            new ContentDialogViewModel<TContent, TResult>(content, actions);
    }

    internal sealed class ContentDialogViewModel<TContent, TResult>
        : ReactiveObject, IContentDialogViewModel, IDialogContentViewModel<TResult>
    {
        internal ContentDialogViewModel(
            TContent content,
            DialogActionSet<TContent, TResult> actions)
        {
            Content = content;

            Actions = DialogActionSetViewModel.Create(actions);

            CloseCommand = ReactiveCommand.Create(
                (ReactiveCommand<TContent, TResult> command) =>
                    command.Execute(Content)
            );

            Result = CloseCommand.AsObservable();
        }

        public TContent Content { get; }

        object? IContentDialogViewModel.Content => Content;

        public DialogActionSetViewModel<TContent, TResult> Actions { get; }

        IEnumerable IContentDialogViewModel.Actions => Actions.Actions;

        public ReactiveCommand<ReactiveCommand<TContent, TResult>, IObservable<TResult>> CloseCommand { get; }

        ICommand IContentDialogViewModel.CloseCommand =>
            CloseCommand;

        public IObservable<IObservable<TResult>> Result { get; }

        public void Close()
        {
            if (Actions.CancelAction is { } cancelAction)
            {
                CloseCommand
                    .Execute(cancelAction.Command)
                    .Subscribe();
            }
        }
    }
}
