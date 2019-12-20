using System;
using System.IO;
using System.Linq;
using System.Windows.Input;

using ReactiveUI;

using Movere.Models;
using Movere.Services;
using File = Movere.Models.File;

namespace Movere.ViewModels
{
    internal sealed class OpenFileDialogViewModel : ReactiveObject
    {
        private readonly Action<OpenFileDialogResult> _closeAction;

        private string _fileName;

        public OpenFileDialogViewModel(
            IFileIconProvider fileIconProvider,
            IClipboardService clipboardService,
            Action<OpenFileDialogResult> closeAction)
        {
            _closeAction = closeAction;

            _fileName = String.Empty;

            FileExplorer = new FileExplorerViewModel(true, fileIconProvider, clipboardService);

            OpenCommand = ReactiveCommand.Create(Open);
            CancelCommand = ReactiveCommand.Create(Cancel);

            FileExplorer.FileOpened.Subscribe(_ => Open());

            FileExplorer.FileExplorerFolder.WhenAnyValue(vm => vm.SelectedItem).Subscribe(SelectedItemChanged);
        }

        public FileExplorerViewModel FileExplorer { get; }

        public string FileName
        {
            get => _fileName;
            set => this.RaiseAndSetIfChanged(ref _fileName, value);
        }

        public ICommand OpenCommand { get; }

        public ICommand CancelCommand { get; }

        private void Open()
        {
            if (FileExplorer.FileExplorerFolder.SelectedItem is Folder folder)
            {
                FileExplorer.NavigateTo(folder);
                return;
            }

            Close(new OpenFileDialogResult(FileExplorer.FileExplorerFolder.SelectedItems.OfType<FileInfo>().Select(info => info.FullName)));
        }

        private void Cancel() => Close(new OpenFileDialogResult(Enumerable.Empty<string>()));

        private void Close(OpenFileDialogResult result) => _closeAction(result);

        private void SelectedItemChanged(FileSystemEntry? entry)
        {
            if (entry is File file)
            {
                FileName = file.Name;
            }
        }
    }
}
