using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

using ReactiveUI;

using Movere.Models;
using Movere.Models.Filters;
using File = Movere.Models.File;

namespace Movere.ViewModels
{
    internal sealed class OpenFileDialogViewModel
        : ReactiveObject, IDialogContentViewModel<OpenFileDialogResult>
    {
        private readonly ISubject<IObservable<OpenFileDialogResult>> _resultSubject =
            new Subject<IObservable<OpenFileDialogResult>>();

        private string _fileName;

        private FileDialogFilterViewModel? _selectedFilter;

        public OpenFileDialogViewModel(
            OpenFileDialogOptions options,
            Func<bool, IObservable<IFilter<FileSystemEntry>>, FileExplorerViewModel> fileExplorerFactory
        )
        {
            Filters = options.Filters.Select(FileDialogFilterViewModel.New).ToImmutableArray();
            SelectedFilter = Filters.FirstOrDefault();

            _fileName = options.InitialFileName ?? String.Empty;

            static IFilter<FileSystemEntry> FileFilterMatches(FileDialogFilterViewModel? x) =>
                Filter.FileDialog.Matches(x?.Filter);

            var filter = this
                .WhenAnyValue(vm => vm.SelectedFilter)
                .Select(FileFilterMatches);

            FileExplorer = fileExplorerFactory(options.AllowMultipleSelection, filter);

            if (options.InitialDirectory is { } initialDirectory)
            {
                FileExplorer.CurrentFolder = new Folder(initialDirectory);
            }

            OpenCommand = ReactiveCommand.Create(Open);
            CancelCommand = ReactiveCommand.Create(Cancel);

            Result = _resultSubject.AsObservable();

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

        public IObservable<IObservable<OpenFileDialogResult>> Result { get; }

        public void Close() =>
            Cancel();

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

        private void Close(OpenFileDialogResult result)
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
