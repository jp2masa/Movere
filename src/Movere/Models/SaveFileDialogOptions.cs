using System.Collections.Generic;
using System.IO;

using Movere.Resources;

namespace Movere.Models
{
    public sealed record SaveFileDialogOptions
    {
        public static SaveFileDialogOptions Default { get; } = new SaveFileDialogOptions();

        public LocalizedString Title { get; init; } =
            new LocalizedString(Strings.ResourceManager, nameof(Strings.SaveFile));

        public string? DefaultExtension { get; init; }

        public IEnumerable<FileDialogFilter> Filters { get; init; } = [];

        public DirectoryInfo? InitialDirectory { get; init; }

        public string? InitialFileName { get; init; }

        public bool ShowOverwritePrompt { get; init; } = true;
    }
}
