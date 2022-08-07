using System.Collections;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using ReactiveUI;

using Movere.Services;
using Movere.Models;

namespace Movere.ViewModels
{
    internal interface IContentDialogViewModel
    {
        LocalizedString Title { get; }

        object? Content { get; }

        IEnumerable Actions { get; }

        ICommand CloseCommand { get; }

        bool OnClosing();
    }

    internal sealed class ContentDialogViewModel<TContent, TResult> : ReactiveObject, IContentDialogViewModel
    {
        private readonly IDialogView<TResult> _view;

        private readonly ObservableAsPropertyHelper<bool> _isBusy;
        private readonly TaskCompletionSource<TResult> _resultTcs = new TaskCompletionSource<TResult>();

        internal ContentDialogViewModel(
            IDialogView<TResult> view,
            LocalizedString title,
            TContent content,
            DialogActionSet<TContent, TResult> actions)
        {
            _view = view;

            Title = title;
            Content = content;

            Actions = DialogActionSetViewModel.Create(actions);

            CloseCommand = ReactiveCommand.CreateFromTask(
                async (ReactiveCommand<TContent, TResult> command) =>
                {
                    var result = await command.Execute(Content);

                    _resultTcs.TrySetResult(result);
                    _view.Close(result);
                }
            );

            _isBusy = CloseCommand.IsExecuting.ToProperty(this, x => x.IsBusy);
        }

        public LocalizedString Title { get; }

        LocalizedString IContentDialogViewModel.Title => Title;

        public TContent Content { get; }

        object? IContentDialogViewModel.Content => Content;

        public DialogActionSetViewModel<TContent, TResult> Actions { get; }

        IEnumerable IContentDialogViewModel.Actions => Actions.Actions;

        public ReactiveCommand<ReactiveCommand<TContent, TResult>, Unit> CloseCommand { get; }

        ICommand IContentDialogViewModel.CloseCommand => CloseCommand;

        public bool IsBusy => _isBusy.Value;

        public bool OnClosing()
        {
            if (_resultTcs.Task.IsCompleted)
            {
                return true;
            }

            if (!IsBusy && Actions.CancelAction is not null)
            {
                _ = CancelAsync(Actions.CancelAction);
            }

            return _resultTcs.Task.IsCompleted;
        }

        private async Task CancelAsync(DialogAction<TContent, TResult> cancelAction) =>
            await CloseCommand.Execute(cancelAction.Command);
    }
}
