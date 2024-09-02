using System;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

using Autofac;

using Movere.Models;
using Movere.ViewModels;

namespace Movere.Services
{
    public sealed class OpenFileDialogService(IDialogHost host) : IOpenFileDialogService
    {
        public async Task<OpenFileDialogResult> ShowDialogAsync(OpenFileDialogOptions? options = null)
        {
            options ??= OpenFileDialogOptions.Default;

            var container = (host as IMovereDialogHost)?.Container
                ?? throw new InvalidOperationException(
                    $"{nameof(OpenFileDialogService)} only supports dialog hosts implemented by Movere!"
                );

            var viewModelFactory = container
                .Resolve<Func<OpenFileDialogOptions, OpenFileDialogViewModel>>();

            return await host
                .ShowDialog<OpenFileDialogViewModel, OpenFileDialogResult>(
                    view =>
                        InternalDialogWindowViewModel.Create(view, options.Title, viewModelFactory(options))
                )
                .ToTask();
        }
    }
}
