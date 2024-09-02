using System.Collections.Generic;
using System.IO;

using Movere.Resources;

namespace Movere.Models
{
    public sealed record OpenFileDialogOptions
    {
        public static OpenFileDialogOptions Default { get; } = new OpenFileDialogOptions();

        public LocalizedString Title { get; init; } =
            new LocalizedString(Strings.ResourceManager, nameof(Strings.OpenFile));

        public bool AllowMultipleSelection { get; init; }

        public IEnumerable<FileDialogFilter> Filters { get; init; } = [];

        public DirectoryInfo? InitialDirectory { get; init; }

        public string? InitialFileName { get; init; }
    };
}
