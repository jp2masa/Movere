using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Controls.Platform;

using Movere.Models;
using Movere.Services;
using Movere.ViewModels;
using MovereOpenFileDialog = Movere.Views.OpenFileDialog;
using MovereSaveFileDialog = Movere.Views.SaveFileDialog;

namespace Movere
{
    internal class MovereSystemDialogImpl : ISystemDialogImpl
    {
        public async Task<string[]> ShowFileDialogAsync(FileDialog dialog, Window parent)
        {
            if (dialog is OpenFileDialog openFileDialog)
            {
                var view = new MovereOpenFileDialog();

                var viewModel = new OpenFileDialogViewModel(
                    openFileDialog.AllowMultiple,
                    view.Close);

                viewModel.FileExplorer.CurrentFolder = new Folder(new DirectoryInfo(Expand(dialog.Directory)));
                viewModel.FileName = Expand(dialog.InitialFileName);

                view.DataContext = viewModel;

                var result = await view.ShowDialog<OpenFileDialogResult>(parent);
                return result == null ? Array.Empty<string>() : result.SelectedPaths.ToArray();
            }

            if (dialog is SaveFileDialog saveFileDialog)
            {
                var view = new MovereSaveFileDialog();

                var viewModel = new SaveFileDialogViewModel(
                    view.Close,
                    new MessageDialogService(view));

                viewModel.FileExplorer.CurrentFolder = new Folder(new DirectoryInfo(Expand(dialog.Directory)));
                viewModel.FileName = Expand(dialog.InitialFileName);

                view.DataContext = viewModel;

                var result = await view.ShowDialog<SaveFileDialogResult>(parent);
                return (result == null || result.SelectedPath == null) ? Array.Empty<string>() : new string[] { result.SelectedPath };
            }

            throw new NotImplementedException();
        }

        public Task<string> ShowFolderDialogAsync(OpenFolderDialog dialog, Window parent) =>
            throw new NotImplementedException();

        private static string Expand(string str) => Environment.ExpandEnvironmentVariables(str ?? String.Empty);
    }
}
