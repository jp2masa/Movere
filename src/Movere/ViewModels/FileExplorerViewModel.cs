using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

using ReactiveUI;

using Movere.Models;
using Movere.Models.Filters;
using Movere.Services;
using File = Movere.Models.File;

namespace Movere.ViewModels
{
#pragma warning disable CA1001 // Types that own disposable fields should be disposable
    public sealed class FileExplorerViewModel : ReactiveObject
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
    {
        private static readonly IObservable<IFilter<FileSystemEntry>> DefaultFilterObservable =
            Observable.Return(Filter.True<FileSystemEntry>());

        private static readonly IReadOnlyList<ItemsView> s_ItemsViews = new ItemsView[]
        {
            ItemsView.List,
            ItemsView.Grid
        };

        private readonly Subject<File> _fileOpened = new Subject<File>();

        private readonly Stack<Folder> _navigationHistoryBack = new Stack<Folder>();
        private readonly Stack<Folder> _navigationHistoryForward = new Stack<Folder>();

        private string _searchText = String.Empty;

        private Folder _currentFolder;

        public FileExplorerViewModel(
            bool allowMultipleSelection,
            IMessageDialogService messageDialogService,
            IObservable<IFilter<FileSystemEntry>>? filter = null,
            IFileIconProvider? fileIconProvider = null,
            IClipboardService? clipboardService = null)
        {
            _currentFolder = new Folder(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)));

            AddressBar = new FileExplorerAddressBarViewModel();

            FileExplorerTree = new FileExplorerTreeViewModel();

            filter ??= DefaultFilterObservable;

            var searchTextFilter =
                from searchText in this.WhenAnyValue(vm => vm.SearchText)
                select Filter.String.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                                    .Cast<string, FileSystemEntry>(e => e.Name);

            filter = Observable.CombineLatest(filter, searchTextFilter, (f, s) => f.And(s));

            FileExplorerFolder = new FileExplorerFolderViewModel(
                allowMultipleSelection, messageDialogService, filter, fileIconProvider, clipboardService);

            FileOpened = _fileOpened.AsObservable();

            NavigateBackCommand = ReactiveCommand.Create(
                NavigateBack,
                this.WhenAnyValue(vm => vm.CanNavigateBack));

            NavigateForwardCommand = ReactiveCommand.Create(
                NavigateForward,
                this.WhenAnyValue(vm => vm.CanNavigateForward));

            NavigateUpCommand = ReactiveCommand.Create(
                NavigateUp,
                this.WhenAnyValue(vm => vm.CurrentFolder).Select(x => x.Parent != null));

            AddressBar.AddressChanged.Subscribe(address => NavigateToAddress(address));

            FileExplorerTree.SelectedFolderChanged.Subscribe(folder => NavigateTo(folder));
            FileExplorerFolder.FolderOpened.Subscribe(folder => NavigateTo(folder));

            FileOpened = FileExplorerFolder.FileOpened;

            this.WhenAnyValue(vm => vm.CurrentFolder)
                .Subscribe(CurrentFolderChanged);
        }

        public FileExplorerAddressBarViewModel AddressBar { get; }

        public FileExplorerTreeViewModel FileExplorerTree { get; }

        public FileExplorerFolderViewModel FileExplorerFolder { get; }

        [SuppressMessage("Performance", "CA1822:Mark members as static")]
        public IReadOnlyList<ItemsView> ItemsViews => s_ItemsViews;

        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        public Folder CurrentFolder
        {
            get => _currentFolder;
            set => this.RaiseAndSetIfChanged(ref _currentFolder, value);
        }

        public IObservable<File> FileOpened { get; }

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
    }
}
