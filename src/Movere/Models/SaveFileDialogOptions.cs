using System.IO;

namespace Movere.Models
{
    public sealed record SaveFileDialogOptions
    {
        public static SaveFileDialogOptions Default { get; } = new SaveFileDialogOptions();
        
        // not supported yet
        //public IEnumerable<FileDialogFilter> Filters { get; init; } = [];

        public DirectoryInfo? InitialDirectory { get; init; }

        public string? InitialFileName { get; init; }
    };
}
