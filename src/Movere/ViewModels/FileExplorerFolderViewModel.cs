using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

using ReactiveUI;

using Movere.Models;

namespace Movere.ViewModels
{
    internal sealed class FileExplorerFolderViewModel : ReactiveObject, IDisposable
    {
        private readonly Subject<FileInfo> _fileOpened;
        private readonly Subject<DirectoryInfo> _folderOpened;

        private Folder? _folder;

        private ObservableCollection<FileSystemInfo> _fileSystemInfos;
        private IDisposable _folderEnumerationDisposable;

        private FileSystemInfo? _selectedItem;

        public FileExplorerFolderViewModel(bool allowMultipleSelection)
        {
            AllowMultipleSelection = allowMultipleSelection;

            _fileSystemInfos = new ObservableCollection<FileSystemInfo>();
            _folderEnumerationDisposable = Disposable.Empty;

            _fileOpened = new Subject<FileInfo>();
            _folderOpened = new Subject<DirectoryInfo>();

            FileOpened = _fileOpened.AsObservable();
            FolderOpened = _folderOpened.AsObservable();

            Items = new ReadOnlyObservableCollection<FileSystemInfo>(_fileSystemInfos);

            SelectedItems = new ObservableCollection<FileSystemInfo>();

            OpenFileCommand = ReactiveCommand.Create<FileInfo>(file => _fileOpened.OnNext(file));
            OpenFolderCommand = ReactiveCommand.Create<DirectoryInfo>(folder => _folderOpened.OnNext(folder));

            this.WhenAnyValue(vm => vm.Folder)
                .Subscribe(CurrentFolderChanged);
        }

        public bool AllowMultipleSelection { get; }

        public Folder? Folder
        {
            get => _folder;
            set => this.RaiseAndSetIfChanged(ref _folder, value);
        }

        public ReadOnlyObservableCollection<FileSystemInfo> Items { get; }

        public FileSystemInfo? SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        public ObservableCollection<FileSystemInfo> SelectedItems { get; }

        public ICommand OpenFileCommand { get; }

        public ICommand OpenFolderCommand { get; }

        public IObservable<FileInfo> FileOpened { get; }

        public IObservable<DirectoryInfo> FolderOpened { get; }

        public void Dispose()
        {
            _fileOpened.Dispose();
            _folderOpened.Dispose();
        }

        private void CurrentFolderChanged(Folder? folder)
        {
            _folderEnumerationDisposable.Dispose();

            _fileSystemInfos.Clear();

            if (Folder == null)
            {
                _folderEnumerationDisposable = Disposable.Empty;
                return;
            }

            _folderEnumerationDisposable = Folder.EnumerateEntries()
                .ToObservable()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(info => _fileSystemInfos.Add(info));
        }
    }
}
