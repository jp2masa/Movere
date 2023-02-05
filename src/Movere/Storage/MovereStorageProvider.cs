using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Autofac;

using Avalonia.Controls;
using Avalonia.Platform.Storage;

using Movere.Models;
using Movere.Services;
using Movere.ViewModels;
using MovereFilter = Movere.Models.FileDialogFilter;
using MovereOpenFileDialog = Movere.Views.OpenFileDialog;
using MovereSaveFileDialog = Movere.Views.SaveFileDialog;

namespace Movere.Storage
{
    internal sealed class MovereStorageProvider : IStorageProvider
    {
        private readonly Window _window;

        public MovereStorageProvider(Window window)
        {
            _window = window;
        }

        public bool CanOpen => true;

        public bool CanSave => true;

        public bool CanPickFolder => false;

        public async Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync(FilePickerOpenOptions options)
        {
            var view = new MovereOpenFileDialog();

            var containerBuilder = new ContainerBuilder();

            containerBuilder
                .RegisterAssemblyModules(typeof(MovereSystemDialogImpl).Assembly);

            containerBuilder
                .RegisterInstance<Window>(view);

            await using var container = containerBuilder.Build();

            view.DataTemplates.Add(container.Resolve<ViewResolver>());

            var viewModel = container.Resolve<Func<bool, IEnumerable<MovereFilter>, OpenFileDialogViewModel>>()(
                options.AllowMultiple,
                (options.FileTypeFilter ?? Array.Empty<FilePickerFileType>())
                    .Select(ConvertFilter)
                    .ToImmutableArray()
            );

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

        public async Task<IStorageFile?> SaveFilePickerAsync(FilePickerSaveOptions options)
        {
            var view = new MovereSaveFileDialog();

            var containerBuilder = new ContainerBuilder();

            containerBuilder
                .RegisterAssemblyModules(typeof(MovereSystemDialogImpl).Assembly);

            containerBuilder
                .RegisterInstance<Window>(view);

            await using var container = containerBuilder.Build();

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

        public Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync(FolderPickerOpenOptions options) =>
            throw new NotSupportedException();

        public Task<IStorageBookmarkFile?> OpenFileBookmarkAsync(string bookmark) =>
            throw new NotSupportedException();

        public Task<IStorageBookmarkFolder?> OpenFolderBookmarkAsync(string bookmark) =>
            throw new NotSupportedException();

        public Task<IStorageFile?> TryGetFileFromPath(Uri filePath) =>
            throw new NotSupportedException();

        public Task<IStorageFolder?> TryGetFolderFromPath(Uri folderPath) =>
            throw new NotSupportedException();

        public Task<IStorageFolder?> TryGetWellKnownFolder(WellKnownFolder wellKnownFolder) =>
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
