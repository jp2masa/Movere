using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using DynamicData;

using ReactiveUI;

using Movere.Models;
using Movere.Models.Filters;
using Movere.Reactive;
using Movere.Services;
using File = Movere.Models.File;

namespace Movere.ViewModels
{
    public sealed class FileExplorerFolderViewModel : ReactiveObject
    {
        private sealed class NullFileIconProvider : IFileIconProvider
        {
            public IFileIcon? GetFileIcon(string filePath) =>
                null;
        }

        private readonly IClipboardService _clipboard;
        private readonly IDialogHost _dialogHost;

        private readonly ObservableAsPropertyHelper<ReadOnlyObservableCollection<FileSystemEntry>> _items;

        private ItemsView _itemsView = ItemsView.List;

        private Folder? _folder;

        private FileSystemEntry? _selectedItem;

        public FileExplorerFolderViewModel(
            IClipboardService clipboard,
            IDialogHost dialogHost,
            bool allowMultipleSelection,
            IFileIconProvider? fileIconProvider = null,
            IObservable<IFilter<FileSystemEntry>>? filter = null)
        {
            _clipboard = clipboard;
            _dialogHost = dialogHost;

            FileIconProvider = fileIconProvider ?? new NullFileIconProvider();

            AllowMultipleSelection = allowMultipleSelection;

            SelectedItems = new ObservableCollection<FileSystemEntry>();

            OpenItemCommand = ReactiveCommand.Create<FileSystemEntry, FileSystemEntry>(x => x);

            FileOpened = OpenItemCommand.OfType<File>();
            FolderOpened = OpenItemCommand.OfType<Folder>();

            CopyCommand = ReactiveCommand.CreateFromTask(CopyAsync);
            DeleteCommand = ReactiveCommand.CreateFromTask(DeleteAsync);

            filter ??= Observable.Return(Filter.True<FileSystemEntry>());

            _items = (
                from folder in this.WhenAnyValue(x => x.Folder)
                select (folder?.Entries ?? Enumerable.Empty<FileSystemEntry>())
                    .ToObservable()
                    .ToObservableChangeSet()
                    .Filter(filter.Select(FilterExtensions.ToFunc), ListFilterPolicy.ClearAndReplace)
                    .SubscribeRoc()
            )
                .ToProperty(this, x => x.Items);
        }

        public IFileIconProvider FileIconProvider { get; }

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

        public ReadOnlyObservableCollection<FileSystemEntry> Items => _items.Value;

        public FileSystemEntry? SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        public ObservableCollection<FileSystemEntry> SelectedItems { get; }

        public ReactiveCommand<FileSystemEntry, FileSystemEntry> OpenItemCommand { get; }

        public ICommand CopyCommand { get; }

        public ICommand DeleteCommand { get; }

        public IObservable<File> FileOpened { get; }

        public IObservable<Folder> FolderOpened { get; }

        private Task CopyAsync() =>
            _clipboard.SetFilesAsync(
                ImmutableArray.CreateRange(
                    from item in SelectedItems
                    select item.FullPath
                )
            );

        private async Task DeleteAsync()
        {
            var result = await _dialogHost.ShowMessageDialogAsync(
                new MessageDialogOptions(
                    $"{SelectedItems.Count} item(s) will be removed permanently. Continue?",
                    (LocalizedString)"Delete items"
                )
                {
                    Icon = AvaloniaDialogIcon.Warning,
                    DialogResults = DialogResultSet.YesNo
                }
            );

            if (result == DialogResult.Yes)
            {
                await Task.WhenAll(
                    from item in SelectedItems
                    select item.DeleteAsync()
                );
            }
        }
    }
}
