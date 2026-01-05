using System;
using System.Drawing.Printing;

using Movere.Resources;

namespace Movere.Models
{
    public sealed record PrintDialogOptions(PrintDocument Document)
    {
        public PrintDocument Document { get; init; } =
            Document ?? throw new ArgumentNullException(nameof(Document));

        public LocalizedString Title { get; init; } =
            new LocalizedString(Strings.ResourceManager, nameof(Strings.Print));
    }
}
