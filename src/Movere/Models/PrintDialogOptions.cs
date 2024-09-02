using System;
using System.Drawing.Printing;

namespace Movere.Models
{
    public sealed record PrintDialogOptions
    {
        public PrintDialogOptions(PrintDocument document)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public PrintDocument Document { get; init; }
    }
}
