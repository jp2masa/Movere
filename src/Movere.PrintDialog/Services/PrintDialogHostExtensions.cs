using System;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

using Autofac;

using Movere.Models;
using Movere.ViewModels;

namespace Movere.Services
{
    public static class PrintDialogHostExtensions
    {
        public static Task<bool> ShowPrintDialogAsync(
            this IDialogHost @this,
            PrintDialogOptions options
        ) =>
            @this
                .ShowPrintDialog(options)
                .ToTask();

        public static IObservable<bool> ShowPrintDialog(
            this IDialogHost @this,
            PrintDialogOptions options
        ) =>
            @this
                .ShowDialog<PrintDialogViewModel, bool>(
                    view =>
                        InternalDialogWindowViewModel.Create(
                            view,
                            options.Title,
                            (view as IMovereDialogView<bool>)?.LifetimeScope
                                .Resolve<Func<PrintDialogOptions, PrintDialogViewModel>>()
                                (options)
                            ?? throw new InvalidOperationException(
                                $"{nameof(PrintDialogHostExtensions)} only supports dialog hosts implemented by Movere!"
                            )
                        )
                );
    }
}
