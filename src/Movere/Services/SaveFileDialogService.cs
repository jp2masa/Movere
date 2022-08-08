using System.Threading.Tasks;

using Autofac;

using Avalonia.Controls;

using Movere.Models;
using Movere.ViewModels;
using SaveFileDialog = Movere.Views.SaveFileDialog;

namespace Movere.Services
{
    public sealed class SaveFileDialogService : ISaveFileDialogService
    {
        private readonly Window _owner;

        public SaveFileDialogService(Window owner)
        {
            _owner = owner;
        }

        public async Task<SaveFileDialogResult> ShowDialogAsync()
        {
            var dialog = new SaveFileDialog();

            var containerBuilder = new ContainerBuilder();

            containerBuilder
                .RegisterAssemblyModules(typeof(SaveFileDialogService).Assembly);

            containerBuilder
                .RegisterInstance<Window>(dialog);

            using var container = containerBuilder.Build();

            dialog.DataTemplates.Add(container.Resolve<ViewResolver>());

            var viewModel = container.Resolve<SaveFileDialogViewModel>();

            dialog.DataContext = viewModel;

            return await dialog.ShowDialog<SaveFileDialogResult>(_owner);
        }
    }
}
