using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;

using ReactiveUI;

using Movere.Models;
using Movere.Models.Filters;
using Movere.Services;
using File = Movere.Models.File;

namespace Movere.ViewModels
{
    internal sealed class OpenFileDialogViewModel : ReactiveObject
    {
        private readonly IDialogView<OpenFileDialogResult> _view;

        private string _fileName;

        private FileDialogFilterViewModel? _selectedFilter;

        public OpenFileDialogViewModel(
            IDialogView<OpenFileDialogResult> view,
            IClipboardService clipboard,
            IFileIconProvider fileIconProvider,
            IMessageDialogService messageDialogService,
            bool allowMultipleSelection,
            IEnumerable<FileDialogFilter> filters)
        {
            _view = view;

            Filters = filters.Select(FileDialogFilterViewModel.New).ToImmutableArray();
            SelectedFilter = Filters.FirstOrDefault();

            _fileName = String.Empty;

            var filter = this.WhenAnyValue(vm => vm.SelectedFilter).Select(vm => vm?.Filter).Select(Filter.FileDialog.Matches);

            FileExplorer = new FileExplorerViewModel(clipboard, fileIconProvider, messageDialogService, allowMultipleSelection, filter);

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

        public IEnumerable<FileDialogFilterViewModel> Filters { get; }

        public FileDialogFilterViewModel? SelectedFilter
        {
            get => _selectedFilter;
            set => this.RaiseAndSetIfChanged(ref _selectedFilter, value);
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

            Close(new OpenFileDialogResult(FileExplorer.FileExplorerFolder.SelectedItems.OfType<File>().Select(info => info.FullPath)));
        }

        private void Cancel() => Close(new OpenFileDialogResult(Enumerable.Empty<string>()));

        private void Close(OpenFileDialogResult result) => _view.Close(result);

        private void SelectedItemChanged(FileSystemEntry? entry)
        {
            if (entry is File file)
            {
                FileName = file.Name;
            }
        }
    }
}
