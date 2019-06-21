using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

using ReactiveUI;

using Movere.Models;

namespace Movere.ViewModels
{
    public sealed class FileExplorerViewModel : ReactiveObject, IDisposable
    {
        private readonly Subject<FileInfo> _fileOpened;

        private readonly Stack<Folder> _navigationHistoryBack;
        private readonly Stack<Folder> _navigationHistoryForward;

        private Folder _currentFolder;

        public FileExplorerViewModel(bool allowMultipleSelection)
        {
            AddressBar = new FileExplorerAddressBarViewModel();

            FileExplorerTree = new FileExplorerTreeViewModel();
            FileExplorerFolder = new FileExplorerFolderViewModel(allowMultipleSelection);

            _navigationHistoryBack = new Stack<Folder>();
            _navigationHistoryForward = new Stack<Folder>();
            
            _fileOpened = new Subject<FileInfo>();
            FileOpened = _fileOpened.AsObservable();

            CurrentFolder = new Folder(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)));

            OpenFileCommand = ReactiveCommand.Create<FileInfo>(file => _fileOpened.OnNext(file));

            OpenFolderCommand = ReactiveCommand.Create<DirectoryInfo>(folder => NavigateTo(new Folder(folder)));

            NavigateBackCommand = ReactiveCommand.Create(
                NavigateBack,
                this.ObservableForProperty(vm => vm.CanNavigateBack).Select(x => x.Value));

            NavigateForwardCommand = ReactiveCommand.Create(
                NavigateForward,
                this.ObservableForProperty(vm => vm.CanNavigateForward).Select(x => x.Value));

            NavigateUpCommand = ReactiveCommand.Create(
                NavigateUp,
                this.ObservableForProperty(vm => vm.CurrentFolder).Select(x => x.Value.Parent != null));

            AddressBar.AddressChanged.Subscribe(address => NavigateToAddress(address));
            AddressBar.AddressPieceOpened.Subscribe(address => NavigateTo(new Folder(address.Directory)));

            FileExplorerTree.SelectedFolderChanged.Subscribe(folder => NavigateTo(folder));
            FileExplorerFolder.FolderOpened.Subscribe(folder => NavigateTo(new Folder(folder)));

            FileOpened = FileExplorerFolder.FileOpened;

            this.WhenAnyValue(vm => vm.CurrentFolder)
                .Subscribe(CurrentFolderChanged);
        }

        public FileExplorerAddressBarViewModel AddressBar { get; }

        public FileExplorerTreeViewModel FileExplorerTree { get; }

        public FileExplorerFolderViewModel FileExplorerFolder{ get; }

        public bool AllowMultipleSelection => true;

        public Folder CurrentFolder
        {
            get => _currentFolder;
            set => this.RaiseAndSetIfChanged(ref _currentFolder, value);
        }

        public IObservable<FileInfo> FileOpened { get; }

        public ICommand OpenFileCommand { get; }

        public ICommand OpenFolderCommand { get; }

        public ICommand NavigateBackCommand { get; }

        public ICommand NavigateForwardCommand { get; }

        public ICommand NavigateUpCommand { get; }

        public bool CanNavigateBack => _navigationHistoryBack.Count > 0;

        public bool CanNavigateForward => _navigationHistoryForward.Count > 0;

        public void NavigateBack()
        {
            if (_navigationHistoryBack.TryPop(out var directory))
            {
                _navigationHistoryForward.Push(CurrentFolder);

                this.RaisePropertyChanged(nameof(CanNavigateBack));
                this.RaisePropertyChanged(nameof(CanNavigateForward));

                NavigateTo(directory, true);
            }
        }

        public void NavigateForward()
        {
            if (_navigationHistoryForward.TryPop(out var directory))
            {
                _navigationHistoryBack.Push(CurrentFolder);

                this.RaisePropertyChanged(nameof(CanNavigateBack));
                this.RaisePropertyChanged(nameof(CanNavigateForward));

                NavigateTo(directory, true);
            }
        }

        public void NavigateUp()
        {
            var parent = CurrentFolder.Parent;

            if (parent != null)
            {
                NavigateTo(parent);
            }
        }

        public void NavigateTo(Folder folder, bool isHistoryNavigation = false)
        {
            if (!CurrentFolder.Equals(folder) && !isHistoryNavigation)
            {
                _navigationHistoryBack.Push(CurrentFolder);
                this.RaisePropertyChanged(nameof(CanNavigateBack));

                _navigationHistoryForward.Clear();
                this.RaisePropertyChanged(nameof(CanNavigateForward));
            }

            CurrentFolder = folder;
        }

        private void NavigateToAddress(string address)
        {
            if (Directory.Exists(address))
            {
                var path = Path.GetFullPath(address);
                var folder = new Folder(new DirectoryInfo(path));

                NavigateTo(folder);

                return;
            }

            if (System.IO.File.Exists(address))
            {
                using (var process = new System.Diagnostics.Process())
                {
                    process.StartInfo.FileName = address;
                    process.StartInfo.UseShellExecute = true;

                    process.Start();
                }
            }

            AddressBar.Address = CurrentFolder.FullPath;
        }

        private void CurrentFolderChanged(Folder folder)
        {
            AddressBar.Address = folder.FullPath;

            FileExplorerTree.SelectedFolder = folder;
            FileExplorerFolder.Folder = folder;
        }

        public void Dispose() => _fileOpened.Dispose();
    }
}
