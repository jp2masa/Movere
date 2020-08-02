using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Input;

using DynamicData;

using ReactiveUI;

using Movere.Models;
using Movere.Models.Filters;
using Movere.Services;
using File = Movere.Models.File;

namespace Movere.ViewModels
{
    public sealed class FileExplorerFolderViewModel : ReactiveObject, IDisposable
    {
        private readonly IFileIconProvider _fileIconProvider;
        private readonly IClipboardService _clipboardService;

        private readonly SourceList<FileSystemEntry> _entries = new SourceList<FileSystemEntry>();
        private readonly ReadOnlyObservableCollection<FileSystemEntry> _items;

        private readonly Subject<File> _fileOpened = new Subject<File>();
        private readonly Subject<Folder> _folderOpened = new Subject<Folder>();

        private ItemsView _itemsView = ItemsView.List;

        private Folder? _folder;

        private IDisposable _folderEnumerationDisposable = Disposable.Empty;

        private FileSystemEntry? _selectedItem;

        public FileExplorerFolderViewModel(
            bool allowMultipleSelection,
            IObservable<IFilter<FileSystemEntry>>? filter = null,
            IFileIconProvider? fileIconProvider = null,
            IClipboardService? clipboardService = null)
        {
            _fileIconProvider = fileIconProvider ?? DefaultFileIconProvider.Instance;
            _clipboardService = clipboardService ?? ClipboardService.Instance;

            AllowMultipleSelection = allowMultipleSelection;

            FileOpened = _fileOpened.AsObservable();
            FolderOpened = _folderOpened.AsObservable();

            filter ??= Observable.Return(Filter.True<FileSystemEntry>());

            _entries.Connect()
                    .Filter(filter.Select(FilterExtensions.ToFunc), ListFilterPolicy.ClearAndReplace)
                    .Bind(out _items)
                    .Subscribe();

            SelectedItems = new ObservableCollection<FileSystemEntry>();

            OpenItemCommand = ReactiveCommand.Create<FileSystemEntry>(ItemOpened);

            CopyCommand = ReactiveCommand.Create(Copy);

            this.WhenAnyValue(vm => vm.Folder)
                .Subscribe(CurrentFolderChanged);
        }

        public IFileIconProvider FileIconProvider => _fileIconProvider;

        public ItemsView ItemsView
        {
            get => _itemsView;
            set => this.RaiseAndSetIfChanged(ref _itemsView, value);
        }

        public bool AllowMultipleSelection { get; }

        public Folder? Folder
        {
            get => _folder;
            set => this.RaiseAndSetIfChanged(ref _folder, value);
        }

        public ReadOnlyObservableCollection<FileSystemEntry> Items => _items;

        public FileSystemEntry? SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        public ObservableCollection<FileSystemEntry> SelectedItems { get; }

        public ICommand OpenItemCommand { get; }

        public ICommand CopyCommand { get; }

        public IObservable<File> FileOpened { get; }

        public IObservable<Folder> FolderOpened { get; }

        public void Dispose()
        {
            _fileOpened.Dispose();
            _folderOpened.Dispose();

            _folderEnumerationDisposable.Dispose();
        }

        private void ItemOpened(FileSystemEntry item)
        {
            switch (item)
            {
                case File file:
                    _fileOpened.OnNext(file);
                    return;
                case Folder folder:
                    _folderOpened.OnNext(folder);
                    return;
            }
        }

        private Task Copy()
        {
            var files = new string[SelectedItems.Count];

            for (int i = 0; i < SelectedItems.Count; i++)
            {
                files[i] = SelectedItems[i].FullPath;
            }

            return _clipboardService.SetFilesAsync(files);
        }

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
