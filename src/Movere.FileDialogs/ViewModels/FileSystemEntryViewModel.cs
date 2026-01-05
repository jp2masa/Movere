using System;

using ReactiveUI;

using Movere.Models;
using Movere.Services;

namespace Movere.ViewModels
{
    public sealed class FileSystemEntryViewModel(
        FileSystemEntry entry,
        IFileIconProvider fileIconProvider
    ) : ReactiveObject
    {
        private readonly Lazy<IFileIcon?> _icon =
            new Lazy<IFileIcon?>(
                () =>
                    entry switch
                    {
                        File file =>
                            fileIconProvider.GetFileIcon(file.FullPath),
                        Folder =>
                            null,
                        _ =>
                            throw new NotSupportedException()
                    }
            );

        public FileSystemEntry Entry =>
            entry;

        public IFileIcon? Icon =>
            _icon.Value;
    }
}
