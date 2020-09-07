using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

using ReactiveUI;

using Movere.Models;
using Movere.Resources;
using Movere.Services;
using File = Movere.Models.File;

namespace Movere.ViewModels
{
    internal sealed class SaveFileDialogViewModel : ReactiveObject
    {
        private readonly IDialogView<SaveFileDialogResult> _view;
        private readonly IMessageDialogService _messageDialogService;

        private string _fileName;

        public SaveFileDialogViewModel(
            IDialogView<SaveFileDialogResult> view,
            IMessageDialogService messageDialogService,
            IFileIconProvider? fileIconProvider = null,
            IClipboardService? clipboardService = null)
        {
            _view = view;
            _messageDialogService = messageDialogService;

            _fileName = String.Empty;

            FileExplorer = new FileExplorerViewModel(false, messageDialogService, null, fileIconProvider, clipboardService);

            SaveCommand = ReactiveCommand.Create(SaveAsync);
            CancelCommand = ReactiveCommand.Create(Cancel);

            FileExplorer.FileOpened.Subscribe(async file => await SaveAsync());

            FileExplorer.FileExplorerFolder.WhenAnyValue(vm => vm.SelectedItem).Subscribe(SelectedItemChanged);
        }

        public FileExplorerViewModel FileExplorer { get; }

        public string FileName
        {
            get => _fileName;
            set => this.RaiseAndSetIfChanged(ref _fileName, value);
        }

        public ICommand SaveCommand { get; }

        public ICommand CancelCommand { get; }

        private async Task SaveAsync()
        {
            if (FileExplorer.FileExplorerFolder.SelectedItem is Folder folder)
            {
                FileExplorer.NavigateTo(folder);
                return;
            }

            var path = FileName;

            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(FileExplorer.CurrentFolder.FullPath, path);
            }

            path = Path.GetFullPath(path);

            if (System.IO.File.Exists(path))
            {
                var result = await _messageDialogService.ShowMessageDialogAsync(
                    new MessageDialogOptions(
                        Strings.FileAlreadyExistsMessage,
                        Strings.Save,
                        DialogIcon.Warning,
                        DialogResultSet.YesNoCancel));

                if (result != DialogResult.Yes)
                {
                    if (result == DialogResult.No)
                    {
                        Cancel();
                    }

                    return;
                }
            }

            Close(new SaveFileDialogResult(FileExplorer.FileExplorerFolder.SelectedItem?.FullPath));
        }

        private void Cancel() => Close(new SaveFileDialogResult(null));

        private void Close(SaveFileDialogResult result) => _view.Close(result);

        private void SelectedItemChanged(FileSystemEntry? entry)
        {
            if (entry is File file)
            {
                FileName = file.Name;
            }
        }
    }
}
