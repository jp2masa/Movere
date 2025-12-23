using Dunet;

namespace Movere.Models
{
    [Union]
    public abstract partial record SaveFileDialogResult
    {
        public sealed partial record Save(string SelectedPath, FileDialogFilter? SelectedFilter);

        public sealed partial record Cancel();
    }
}
