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

        public FileExplorerTreeViewModel()
        {
            _drives = DriveInfo.GetDrives();

            FolderHierarchy = _drives.Select(d => new Folder(d.RootDirectory));

            SelectedFolderChanged = from folder in this.WhenAnyValue(vm => vm.SelectedFolder)
                                    where folder is not null
                                    select folder;
        }

        public IEnumerable<Folder> FolderHierarchy { get; }

        public Folder? SelectedFolder
        {
            get;
            set => this.RaiseAndSetIfChanged(ref field, value);
        }

        public IObservable<Folder> SelectedFolderChanged { get; }
    }
}
