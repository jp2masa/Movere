using System;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

using Movere.Models;
using Movere.ViewModels;

namespace Movere.Services
{
    public static class MessageDialogHostExtensions
    {
        public static Task<DialogResult> ShowMessageDialogAsync(
            this IDialogHost @this,
            MessageDialogOptions options
        ) =>
            @this
                .ShowMessageDialog(options)
                .ToTask();

        public static IObservable<DialogResult> ShowMessageDialog(
            this IDialogHost @this,
            MessageDialogOptions options
        ) =>
            @this
                .ShowContentDialog(
                    new ContentDialogOptions<MessageDialogViewModel, DialogResult>(
                        options.Title,
                        new MessageDialogViewModel(options.Icon, options.Message),
                        DialogActionSet.FromDialogResultSet<MessageDialogViewModel>(options.DialogResults)
                    )
                );
    }
}
