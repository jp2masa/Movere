using System;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

using Autofac;

using Movere.Models;
using Movere.ViewModels;

namespace Movere.Services
{
    public static class SaveFileDialogHostExtensions
    {
        public static Task<SaveFileDialogResult> ShowSaveFileDialogAsync(
            this IDialogHost @this,
            SaveFileDialogOptions? options = null
        ) =>
            @this
                .ShowSaveFileDialog(options)
                .ToTask();

        public static IObservable<SaveFileDialogResult> ShowSaveFileDialog(
            this IDialogHost @this,
            SaveFileDialogOptions? options = null
        )
        {
            options ??= SaveFileDialogOptions.Default;

            return @this
                .ShowDialog<SaveFileDialogViewModel, SaveFileDialogResult>(
                    view =>
                        InternalDialogWindowViewModel.Create(
                            view,
                            options.Title,
                            (view as IMovereDialogView<SaveFileDialogResult>)?.LifetimeScope
                                .Resolve<Func<SaveFileDialogOptions, SaveFileDialogViewModel>>()
                                (options)
                            ?? throw new InvalidOperationException(
                                $"{nameof(SaveFileDialogHostExtensions)} only supports dialog hosts implemented by Movere!"
                            )
                        )
                );
        }
    }
}
