using System;
using System.Threading.Tasks;

using Autofac;

using Avalonia;
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

        public async Task<OpenFileDialogResult> ShowDialogAsync(OpenFileDialogOptions? options = null)
        {
            options ??= OpenFileDialogOptions.Default;

            var dialog = new OpenFileDialog();

            var containerBuilder = new ContainerBuilder()
            {
                Properties = { ["Application"] = Application.Current }
            };

            containerBuilder
                .RegisterAssemblyModules(typeof(OpenFileDialogService).Assembly);

            containerBuilder
                .RegisterInstance<Window>(dialog);

            using var container = containerBuilder.Build();

            dialog.DataTemplates.Add(container.Resolve<ViewResolver>());

            var viewModelFactory = container
                .Resolve<Func<OpenFileDialogOptions, OpenFileDialogViewModel>>();

            dialog.DataContext = viewModelFactory(options);

            return await dialog.ShowDialog<OpenFileDialogResult>(_owner);
        }
    }
}
