using System;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

using Autofac;

using Movere.Models;
using Movere.ViewModels;

namespace Movere.Services
{
    public static class OpenFileDialogHostExtensions
    {
        public static Task<OpenFileDialogResult> ShowOpenFileDialogAsync(
            this IDialogHost @this,
            OpenFileDialogOptions? options = null
        ) =>
            @this
                .ShowOpenFileDialog(options)
                .ToTask();

        public static IObservable<OpenFileDialogResult> ShowOpenFileDialog(
            this IDialogHost @this,
            OpenFileDialogOptions? options = null
        )
        {
            options ??= OpenFileDialogOptions.Default;

            return @this
                .ShowDialog<OpenFileDialogViewModel, OpenFileDialogResult>(
                    view =>
                        InternalDialogWindowViewModel.Create(
                            view,
                            options.Title,
                            (view as IMovereDialogView<OpenFileDialogResult>)?.LifetimeScope
                                .Resolve<Func<OpenFileDialogOptions, OpenFileDialogViewModel>>()
                                (options)
                            ?? throw new InvalidOperationException(
                                $"{nameof(OpenFileDialogHostExtensions)} only supports dialog hosts implemented by Movere!"
                            )
                        )
                );
        }
    }
}
