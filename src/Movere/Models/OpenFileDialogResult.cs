using System.Collections.Generic;

namespace Movere.Models
{
    public sealed class OpenFileDialogResult
    {
        public OpenFileDialogResult(IEnumerable<string> selectedPaths)
        {
            SelectedPaths = selectedPaths;
        }

        public IEnumerable<string> SelectedPaths { get; }
    }
}
