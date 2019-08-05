using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Input;

using ReactiveUI;

using Movere.Models;
using Movere.Services;
using File = Movere.Models.File;

namespace Movere.ViewModels
{
    public sealed class FileExplorerFolderViewModel : ReactiveObject, IDisposable
    {
        private readonly Subject<File> _fileOpened;
        private readonly Subject<Folder> _folderOpened;

        private Folder? _folder;

        private ObservableCollection<FileSystemEntry> _entries;
        private IDisposable _folderEnumerationDisposable;

        private FileSystemEntry? _selectedItem;

        public FileExplorerFolderViewModel(bool allowMultipleSelection)
        {
            AllowMultipleSelection = allowMultipleSelection;

            _entries = new ObservableCollection<FileSystemEntry>();
            _folderEnumerationDisposable = Disposable.Empty;

            _fileOpened = new Subject<File>();
            _folderOpened = new Subject<Folder>();

            FileOpened = _fileOpened.AsObservable();
            FolderOpened = _folderOpened.AsObservable();

            Items = new ReadOnlyObservableCollection<FileSystemEntry>(_entries);

            SelectedItems = new ObservableCollection<FileSystemEntry>();

            OpenFileCommand = ReactiveCommand.Create<File>(file => _fileOpened.OnNext(file));
            OpenFolderCommand = ReactiveCommand.Create<Folder>(folder => _folderOpened.OnNext(folder));

            CopyCommand = ReactiveCommand.Create(Copy);

            this.WhenAnyValue(vm => vm.Folder)
                .Subscribe(CurrentFolderChanged);
        }

        public bool AllowMultipleSelection { get; }

        public Folder? Folder
        {
            get => _folder;
            set => this.RaiseAndSetIfChanged(ref _folder, value);
        }

        public ReadOnlyObservableCollection<FileSystemEntry> Items { get; }

        public FileSystemEntry? SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        public ObservableCollection<FileSystemEntry> SelectedItems { get; }

        public ICommand OpenFileCommand { get; }

        public ICommand OpenFolderCommand { get; }

        public ICommand CopyCommand { get; }

        public IObservable<File> FileOpened { get; }

        public IObservable<Folder> FolderOpened { get; }

        public void Dispose()
        {
            _fileOpened.Dispose();
            _folderOpened.Dispose();

            _folderEnumerationDisposable.Dispose();
        }

        private Task Copy() => Task.CompletedTask;

        private void CurrentFolderChanged(Folder? folder)
        {
            _folderEnumerationDisposable.Dispose();

            _entries.Clear();

            if (Folder == null)
            {
                _folderEnumerationDisposable = Disposable.Empty;
                return;
            }

            _folderEnumerationDisposable = Folder.Entries
                .ToObservable()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(entry => _entries.Add(entry));
        }
    }
}
