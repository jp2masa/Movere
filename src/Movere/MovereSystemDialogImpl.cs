using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
    internal sealed class MovereSystemDialogImpl : ISystemDialogImpl
    {
        public async Task<string[]> ShowFileDialogAsync(FileDialog dialog, Window parent)
        {
            if (dialog is OpenFileDialog openFileDialog)
            {
                var view = new MovereOpenFileDialog() { DataTemplates = { ViewResolver.Default } };

                var viewModel = new OpenFileDialogViewModel(
                    new DialogView<OpenFileDialogResult>(view),
                    ClipboardService.Instance,
                    DefaultFileIconProvider.Instance,
                    new MessageDialogService(view),
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
                return result == null ? Array.Empty<string>() : result.SelectedPaths.ToArray();
            }

            if (dialog is SaveFileDialog saveFileDialog)
            {
                var view = new MovereSaveFileDialog() { DataTemplates = { ViewResolver.Default } };

                var viewModel = new SaveFileDialogViewModel(
                    new DialogView<SaveFileDialogResult>(view),
                    ClipboardService.Instance,
                    DefaultFileIconProvider.Instance,
                    new MessageDialogService(view));

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
                return (result == null || result.SelectedPath == null) ? Array.Empty<string>() : new string[] { result.SelectedPath };
            }

            throw new NotImplementedException();
        }

        public Task<string> ShowFolderDialogAsync(OpenFolderDialog dialog, Window parent) =>
            throw new NotImplementedException();

        private static MovereFilter ConvertFilter(AvaloniaFilter filter) =>
            new MovereFilter(filter.Name, filter.Extensions.ToImmutableArray());
    }
}
