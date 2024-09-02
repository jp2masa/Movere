using System;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

using Autofac;

using Movere.Models;
using Movere.ViewModels;

namespace Movere.Services
{
    public sealed class SaveFileDialogService(IDialogHost host) : ISaveFileDialogService
    {
        public Task<SaveFileDialogResult> ShowDialogAsync(SaveFileDialogOptions? options = null)
        {
            options ??= SaveFileDialogOptions.Default;

            var container = (host as IMovereDialogHost)?.Container
                ?? throw new InvalidOperationException(
                    $"{nameof(SaveFileDialogService)} only supports dialog hosts implemented by Movere!"
                );
            
            var viewModelFactory = container
                .Resolve<Func<SaveFileDialogOptions, SaveFileDialogViewModel>>();

            return host
                .ShowDialog<SaveFileDialogViewModel, SaveFileDialogResult>(
                    view =>
                        InternalDialogWindowViewModel.Create(view, options.Title, viewModelFactory(options))
                )
                .ToTask();
        }
    }
}
