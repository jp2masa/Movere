using System;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

using Autofac;

using Movere.Models;
using Movere.ViewModels;

namespace Movere.Services
{
    public sealed class PrintDialogService(IDialogHost host) : IPrintDialogService
    {
        public Task<bool> ShowDialogAsync(PrintDialogOptions options)
        {
            var container = (host as IMovereDialogHost)?.Container
                ?? throw new InvalidOperationException(
                    $"{nameof(PrintDialogService)} only supports dialog hosts implemented by Movere!"
                );

            var viewModelFactory = container
                .Resolve<Func<PrintDialogOptions, PrintDialogViewModel>>();

            return host
                .ShowDialog<PrintDialogViewModel, bool>(
                    view =>
                        InternalDialogWindowViewModel.Create(view, options.Title, viewModelFactory(options))
                )
                .ToTask();
        }
    }
}
