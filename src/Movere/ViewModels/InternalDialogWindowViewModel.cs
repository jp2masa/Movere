using System;
using System.Reactive.Linq;

using ReactiveUI;

using Movere.Models;
using Movere.Services;

namespace Movere.ViewModels
{
    internal static class InternalDialogWindowViewModel
    {
        public static InternalDialogWindowViewModel<TContent, TResult> Create<TContent, TResult>(
            IDialogView<TResult> view,
            LocalizedString title,
            TContent content
        )
            where TContent : IDialogContentViewModel<TResult> =>
            new InternalDialogWindowViewModel<TContent, TResult>(view, title, content);
    }

    internal sealed class InternalDialogWindowViewModel<TContent, TResult> : ReactiveObject, IDialogWindowViewModel
        where TContent : IDialogContentViewModel<TResult>
    {
        private readonly IDialogView<TResult> _view;

        private readonly ObservableAsPropertyHelper<bool> _isBusy;

        private bool _isCompleted = false;

        public InternalDialogWindowViewModel(
            IDialogView<TResult> view,
            LocalizedString title,
            TContent content
        )
        {
            _view = view;

            Title = title;
            Content = content;

            _isBusy = Content.Result
                .Scan(false, (_, _) => true)
                .ToProperty(this, x => x.IsBusy);

            Content.Result
                .Concat()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(Close);
        }

        public LocalizedString Title { get; }

        public TContent Content { get; }

        object IDialogWindowViewModel.Content =>
            Content;

        public bool IsBusy =>
            _isBusy.Value;

        public bool OnClosing()
        {
            if (_isCompleted)
            {
                return true;
            }

            if (!IsBusy)
            {
                Content.Close();
            }

            return _isCompleted;
        }

        private void Close(TResult result)
        {
            _isCompleted = true;
            _view.Close(result);
        }
    }
}
