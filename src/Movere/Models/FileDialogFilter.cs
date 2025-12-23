using System.Collections.Immutable;

namespace Movere.Models
{
    public sealed record FileDialogFilter(
        string Name,
        ImmutableArray<string> Extensions
    );
}
