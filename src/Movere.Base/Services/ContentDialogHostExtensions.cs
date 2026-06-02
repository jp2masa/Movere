using System;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

using Movere.ViewModels;

namespace Movere.Services
{
    public static class ContentDialogHostExtensions
    {
        public static Task<TResult> ShowContentDialogAsync<TContent, TResult>(
            this IDialogHost @this,
            ContentDialogOptions<TContent, TResult> options
        ) =>
            @this
                .ShowContentDialog(options)
                .ToTask();

        public static IObservable<TResult> ShowContentDialog<TContent, TResult>(
            this IDialogHost @this,
            ContentDialogOptions<TContent, TResult> options
        ) =>
            @this
                .ShowDialog(
                    (IDialogView<TResult> view) =>
                        InternalDialogWindowViewModel.Create(
                            view,
                            options.Title,
                            ContentDialogViewModel.Create(
                                options.Content,
                                options.Actions
                            )
                        )
                );
    }
}
