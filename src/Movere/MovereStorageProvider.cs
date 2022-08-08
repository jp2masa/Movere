using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Autofac;

using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Platform.Storage.FileIO;

using Movere.Models;
using Movere.Services;
using Movere.ViewModels;
using MovereFilter = Movere.Models.FileDialogFilter;
using MovereOpenFileDialog = Movere.Views.OpenFileDialog;
using MovereSaveFileDialog = Movere.Views.SaveFileDialog;

namespace Movere
{
    internal sealed class MovereStorageProvider : BclStorageProvider
    {
        private Window _window;

        public MovereStorageProvider(Window window)
        {
            _window = window;
        }

        public override bool CanOpen => true;

        public override bool CanSave => true;

        public override bool CanPickFolder => false;

        public override async Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync(FilePickerOpenOptions options)
        {
            var view = new MovereOpenFileDialog();

            var containerBuilder = new ContainerBuilder();

            containerBuilder
                .RegisterAssemblyModules(typeof(MovereSystemDialogImpl).Assembly);

            containerBuilder
                .RegisterInstance<Window>(view);

            using var container = containerBuilder.Build();

            view.DataTemplates.Add(container.Resolve<ViewResolver>());

            var viewModel = container.Resolve<Func<bool, IEnumerable<MovereFilter>, OpenFileDialogViewModel>>()(
                options.AllowMultiple,
                options.FileTypeFilter.Select(ConvertFilter).ToImmutableArray());

            var suggestedStartLocation = await (options.SuggestedStartLocation?.SaveBookmarkAsync() ?? Task.FromResult<string?>(null));

            if (!String.IsNullOrWhiteSpace(suggestedStartLocation))
            {
                var directory = new DirectoryInfo(suggestedStartLocation);

                if (directory.Exists)
                {
                    viewModel.FileExplorer.CurrentFolder = new Folder(directory);
                }
            }

            view.DataContext = viewModel;

            var result = await view.ShowDialog<OpenFileDialogResult>(_window);
            return (
                from path in result?.SelectedPaths ?? Enumerable.Empty<string>()
                select new BclStorageFile(new FileInfo(path))
            )
                .ToImmutableArray();
        }

        public override async Task<IStorageFile?> SaveFilePickerAsync(FilePickerSaveOptions options)
        {
            var view = new MovereSaveFileDialog();

            var containerBuilder = new ContainerBuilder();

            containerBuilder
                .RegisterAssemblyModules(typeof(MovereSystemDialogImpl).Assembly);

            containerBuilder
                .RegisterInstance<Window>(view);

            using var container = containerBuilder.Build();

            view.DataTemplates.Add(container.Resolve<ViewResolver>());

            var viewModel = container.Resolve<SaveFileDialogViewModel>();

            var suggestedStartLocation = await (options.SuggestedStartLocation?.SaveBookmarkAsync() ?? Task.FromResult<string?>(null));

            if (!String.IsNullOrWhiteSpace(suggestedStartLocation))
            {
                var directory = new DirectoryInfo(suggestedStartLocation);

                if (directory.Exists)
                {
                    viewModel.FileExplorer.CurrentFolder = new Folder(directory);
                }
            }

            if (!(options.SuggestedFileName is null))
            {
                viewModel.FileName = options.SuggestedFileName;
            }

            view.DataContext = viewModel;

            var result = await view.ShowDialog<SaveFileDialogResult>(_window);
            return (result is null || result.SelectedPath is null) ? null : new BclStorageFile(new FileInfo(result.SelectedPath));
        }

        public override Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync(FolderPickerOpenOptions options) =>
            throw new NotSupportedException();

        private static MovereFilter ConvertFilter(FilePickerFileType filter) =>
            new MovereFilter(filter.Name, GetExtensions(filter));

        private static ImmutableArray<string> GetExtensions(FilePickerFileType filter) =>
            (
                filter.Patterns
                    ?? filter.MimeTypes
                    ?? filter.AppleUniformTypeIdentifiers
                    ?? Array.Empty<string>()
            )
                .ToImmutableArray();
    }
}
