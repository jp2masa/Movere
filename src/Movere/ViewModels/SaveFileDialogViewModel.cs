using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Input;

using Movere.Models;
using Movere.Models.Filters;
using Movere.Resources;
using Movere.Services;

using ReactiveUI;

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

        private FileDialogFilterViewModel? _selectedFilter;

        public SaveFileDialogViewModel(
            SaveFileDialogOptions options,
            Func<bool, IObservable<IFilter<FileSystemEntry>>, FileExplorerViewModel> fileExplorerFactory,
            IDialogHost dialogHost
        )
        {
            _dialogHost = dialogHost;

            Filters = options.Filters.Select(FileDialogFilterViewModel.New).ToImmutableArray();

            SelectedFilter = (
                options.DefaultExtension is null
                    ? null
                    : Filters.FirstOrDefault(
                        x => x.Filter.Extensions
                            .Contains(options.DefaultExtension, StringComparer.OrdinalIgnoreCase)
                    )
            )
                ?? Filters.FirstOrDefault(x => x.Filter.Extensions.Contains("*"))
                ?? Filters.FirstOrDefault();

            _showOverwritePrompt = options.ShowOverwritePrompt;

            _fileName = options.InitialFileName is null
                ? String.Empty
                : (
                    options.InitialFileName.EndsWith($".{options.DefaultExtension}")
                        ? options.InitialFileName
                        : $"{options.InitialFileName}.{options.DefaultExtension}"
                );

            static IFilter<FileSystemEntry> FileFilterMatches(FileDialogFilterViewModel? x) =>
                Filter.FileDialog.Matches(x?.Filter);

            var filter = this
                .WhenAnyValue(vm => vm.SelectedFilter)
                .Select(FileFilterMatches);

            FileExplorer = fileExplorerFactory(false, filter);

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
        public IEnumerable<FileDialogFilterViewModel> Filters { get; }

        public FileDialogFilterViewModel? SelectedFilter
        {
            get => _selectedFilter;
            set => this.RaiseAndSetIfChanged(ref _selectedFilter, value);
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
                Close(new SaveFileDialogResult(path, SelectedFilter?.Filter));
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

        private void Cancel() =>
            Close(new SaveFileDialogResult(null, SelectedFilter?.Filter));

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
