using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Autofac;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Platform;
using AvaloniaFilter = Avalonia.Controls.FileDialogFilter;

using Movere.Models;
using Movere.Services;
using Movere.ViewModels;
using MovereFilter = Movere.Models.FileDialogFilter;
using MovereOpenFileDialog = Movere.Views.OpenFileDialog;
using MovereSaveFileDialog = Movere.Views.SaveFileDialog;

namespace Movere
{
    [Obsolete]
    internal sealed class MovereSystemDialogImpl : ISystemDialogImpl
    {
        public async Task<string[]?> ShowFileDialogAsync(FileDialog dialog, Window parent)
        {
            if (dialog is OpenFileDialog openFileDialog)
            {
                var view = new MovereOpenFileDialog();

                var containerBuilder = new ContainerBuilder()
                {
                    Properties = { ["Application"] = Application.Current }
                };

                containerBuilder
                    .RegisterAssemblyModules(typeof(MovereSystemDialogImpl).Assembly);

                containerBuilder
                    .RegisterInstance<Window>(view);

                using var container = containerBuilder.Build();

                view.DataTemplates.Add(container.Resolve<ViewResolver>());

                var viewModel = container.Resolve<Func<bool, IEnumerable<MovereFilter>, OpenFileDialogViewModel>>()(
                    openFileDialog.AllowMultiple,
                    dialog.Filters.Select(ConvertFilter).ToImmutableArray());

                if (!String.IsNullOrWhiteSpace(dialog.Directory))
                {
                    var directory = new DirectoryInfo(dialog.Directory);

                    if (directory.Exists)
                    {
                        viewModel.FileExplorer.CurrentFolder = new Folder(directory);
                    }
                }

                if (!(dialog.InitialFileName is null))
                {
                    viewModel.FileName = dialog.InitialFileName;
                }

                view.DataContext = viewModel;

                var result = await view.ShowDialog<OpenFileDialogResult>(parent);
                return result is null ? Array.Empty<string>() : result.SelectedPaths.ToArray();
            }

            if (dialog is SaveFileDialog saveFileDialog)
            {
                var view = new MovereSaveFileDialog();

                var containerBuilder = new ContainerBuilder()
                {
                    Properties = { ["Application"] = Application.Current }
                };

                containerBuilder
                    .RegisterAssemblyModules(typeof(MovereSystemDialogImpl).Assembly);

                containerBuilder
                    .RegisterInstance<Window>(view);

                using var container = containerBuilder.Build();

                view.DataTemplates.Add(container.Resolve<ViewResolver>());

                var viewModel = container.Resolve<SaveFileDialogViewModel>();

                if (!String.IsNullOrWhiteSpace(dialog.Directory))
                {
                    var directory = new DirectoryInfo(dialog.Directory);

                    if (directory.Exists)
                    {
                        viewModel.FileExplorer.CurrentFolder = new Folder(directory);
                    }
                }

                if (!(dialog.InitialFileName is null))
                {
                    viewModel.FileName = dialog.InitialFileName;
                }

                view.DataContext = viewModel;

                var result = await view.ShowDialog<SaveFileDialogResult>(parent);
                return (result is null || result.SelectedPath is null) ? Array.Empty<string>() : new string[] { result.SelectedPath };
            }

            throw new NotImplementedException();
        }

        public Task<string?> ShowFolderDialogAsync(OpenFolderDialog dialog, Window parent) =>
            throw new NotImplementedException();

        private static MovereFilter ConvertFilter(AvaloniaFilter filter) =>
            new MovereFilter(filter.Name ?? String.Join(", ", filter.Extensions), filter.Extensions.ToImmutableArray());
    }
}
