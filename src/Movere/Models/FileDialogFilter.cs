using System.Collections.Immutable;

namespace Movere.Models
{
    public sealed class FileDialogFilter
    {
        public FileDialogFilter(string name, ImmutableArray<string> extensions)
        {
            Name = name;
            Extensions = extensions;
        }

        public string Name { get; }

        public ImmutableArray<string> Extensions { get; }
    }
}
