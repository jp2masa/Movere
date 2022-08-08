using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Autofac;

using Window = Avalonia.Controls.Window;

using Movere.Models;
using Movere.ViewModels;
using Movere.Views;

namespace Movere.Services
{
    public sealed class OpenFileDialogService : IOpenFileDialogService
    {
        private readonly Window _owner;

        public OpenFileDialogService(Window owner)
        {
            _owner = owner;
        }

        public async Task<OpenFileDialogResult> ShowDialogAsync(bool allowMultipleSelection = false)
        {
            var dialog = new OpenFileDialog();

            var containerBuilder = new ContainerBuilder();

            containerBuilder
                .RegisterAssemblyModules(typeof(OpenFileDialogService).Assembly);

            containerBuilder
                .RegisterInstance<Window>(dialog);

            using var container = containerBuilder.Build();

            dialog.DataTemplates.Add(container.Resolve<ViewResolver>());

            var viewModel = container.Resolve<Func<bool, IEnumerable<FileDialogFilter>, OpenFileDialogViewModel>>()(
                allowMultipleSelection,
                Enumerable.Empty<FileDialogFilter>());

            dialog.DataContext = viewModel;

            return await dialog.ShowDialog<OpenFileDialogResult>(_owner);
        }
    }
}
