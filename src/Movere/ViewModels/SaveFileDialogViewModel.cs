using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Input;

using ReactiveUI;

using Movere.Models;
using Movere.Resources;
using Movere.Services;
using File = Movere.Models.File;

namespace Movere.ViewModels
{
    internal sealed class SaveFileDialogViewModel
        : ReactiveObject, IDialogContentViewModel<SaveFileDialogResult>
    {
        private static readonly MessageDialogOptions s_fileAlreadyExistsMessageDialogOptions =
            new MessageDialogOptions(
                new LocalizedString(Strings.ResourceManager, nameof(Strings.FileAlreadyExistsMessage)),
                new LocalizedString(Strings.ResourceManager, nameof(Strings.Save))
            )
            {
                Icon = AvaloniaDialogIcon.Warning,
                DialogResults = DialogResultSet.YesNoCancel
            };

        private readonly IDialogHost _dialogHost;

        private readonly bool _showOverwritePrompt;

        private readonly ISubject<IObservable<SaveFileDialogResult>> _resultSubject =
            new Subject<IObservable<SaveFileDialogResult>>();

        private string _fileName;

        public SaveFileDialogViewModel(
            SaveFileDialogOptions options,
            Func<bool, FileExplorerViewModel> fileExplorerFactory,
            IDialogHost dialogHost
        )
        {
            _dialogHost = dialogHost;

            _showOverwritePrompt = options.ShowOverwritePrompt;

            _fileName = options.InitialFileName ?? String.Empty;

            FileExplorer = fileExplorerFactory(false);

            if (options.InitialDirectory is { } initialDirectory)
            {
                FileExplorer.CurrentFolder = new Folder(initialDirectory);
            }

            SaveCommand = ReactiveCommand.Create(SaveAsync);
            CancelCommand = ReactiveCommand.Create(Cancel);

            Result = _resultSubject.AsObservable();

            FileExplorer.FileOpened.Subscribe(async file => await SaveAsync());

            FileExplorer.FileExplorerFolder
                .WhenAnyValue(vm => vm.SelectedItem)
                .Select(x => x?.Entry)
                .Subscribe(SelectedItemChanged);
        }

        public FileExplorerViewModel FileExplorer { get; }

        public string FileName
        {
            get => _fileName;
            set => this.RaiseAndSetIfChanged(ref _fileName, value);
        }

        public ICommand SaveCommand { get; }

        public ICommand CancelCommand { get; }

        public IObservable<IObservable<SaveFileDialogResult>> Result { get; }

        public void Close() =>
            Cancel();

        private async Task SaveAsync()
        {
            if (FileExplorer.FileExplorerFolder.SelectedItem?.Entry is Folder folder)
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

            var result = _showOverwritePrompt && System.IO.File.Exists(path)
                ? await _dialogHost
                    .ShowMessageDialogAsync(s_fileAlreadyExistsMessageDialogOptions)
                : DialogResult.Yes;

            if (result == DialogResult.Yes)
            {
                Close(new SaveFileDialogResult(path));
            }
            else if (result == DialogResult.No)
            {
                Cancel();
            }
            else if (result != DialogResult.Cancel)
            {
                throw new InvalidOperationException();
            }
        }

        private void Cancel() => Close(new SaveFileDialogResult(null));

        private void Close(SaveFileDialogResult result)
        {
            _resultSubject.OnNext(Observable.Return(result));
            _resultSubject.OnCompleted();
        }

        private void SelectedItemChanged(FileSystemEntry? entry)
        {
            if (entry is File file)
            {
                FileName = file.Name;
            }
        }
    }
}
