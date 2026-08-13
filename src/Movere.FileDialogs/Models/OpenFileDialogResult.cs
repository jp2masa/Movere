using System.Collections.Immutable;

using Dunet;

namespace Movere.Models
{
    [Union]
    public abstract partial record OpenFileDialogResult
    {
        public sealed partial record Open(ImmutableArray<string> SelectedPaths, FileDialogFilter? SelectedFilter);

        public sealed partial record Cancel();
    }
}
