namespace Movere.Models
{
    public sealed class SaveFileDialogResult
    {
        public SaveFileDialogResult(string? selectedPath, FileDialogFilter? selectedFilter)
        {
            SelectedPath = selectedPath;
            SelectedFilter = selectedFilter;
        }

        public string? SelectedPath { get; }

        public FileDialogFilter? SelectedFilter { get; }
    }
}
