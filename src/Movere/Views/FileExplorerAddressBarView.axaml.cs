using Avalonia.ReactiveUI;

using Movere.ViewModels;

namespace Movere.Views
{
    internal sealed partial class FileExplorerAddressBarView : ReactiveUserControl<FileExplorerAddressBarViewModel>
    {
        public FileExplorerAddressBarView()
        {
            InitializeComponent();
        }
    }
}
