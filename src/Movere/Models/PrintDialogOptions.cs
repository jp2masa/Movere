using System;
using System.Drawing.Printing;

using Movere.Resources;

namespace Movere.Models
{
    public sealed record PrintDialogOptions
    {
        public PrintDialogOptions(PrintDocument document)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public PrintDocument Document { get; init; }

        public LocalizedString Title { get; init; } =
            new LocalizedString(Strings.ResourceManager, nameof(Strings.Print));
    }
}
