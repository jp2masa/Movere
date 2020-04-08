namespace Movere.Models
{
    public sealed class SaveFileDialogResult
    {
        public SaveFileDialogResult(string? selectedPath)
        {
            SelectedPath = selectedPath;
        }

        public string? SelectedPath { get; }
    }
}
