using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

using ReactiveUI;

using Movere.Models;

namespace Movere.ViewModels
{
    public sealed class FileExplorerTreeViewModel : ReactiveObject
    {
        private readonly DriveInfo[] _drives;
        private Folder? _selectedFolder;

        public FileExplorerTreeViewModel()
        {
            _drives = DriveInfo.GetDrives();

            FolderHierarchy = _drives.Select(d => new Folder(d.RootDirectory));

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            SelectedFolderChanged = this.WhenAnyValue(vm => vm.SelectedFolder).Where(folder => folder != null);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }

        public IEnumerable<Folder> FolderHierarchy { get; }

        public Folder? SelectedFolder
        {
            get => _selectedFolder;
            set => this.RaiseAndSetIfChanged(ref _selectedFolder, value);
        }

        public IObservable<Folder> SelectedFolderChanged { get; }
    }
}
