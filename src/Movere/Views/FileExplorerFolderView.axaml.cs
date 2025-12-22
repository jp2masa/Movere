using ReactiveUI.Avalonia;

using Movere.ViewModels;

namespace Movere.Views
{
    internal sealed partial class FileExplorerFolderView : ReactiveUserControl<FileExplorerFolderViewModel>
    {
        public FileExplorerFolderView()
        {
            InitializeComponent();
        }
    }
}
