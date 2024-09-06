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
        )
        {
            var container = (@this as IMovereDialogHost)?.Container
                ?? throw new InvalidOperationException(
                    $"{nameof(PrintDialogHostExtensions)} only supports dialog hosts implemented by Movere!"
                );

            var viewModelFactory = container
                .Resolve<Func<PrintDialogOptions, PrintDialogViewModel>>();

            return @this
                .ShowDialog<PrintDialogViewModel, bool>(
                    view =>
                        InternalDialogWindowViewModel.Create(view, options.Title, viewModelFactory(options))
                );
        }
    }
}
