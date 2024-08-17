using Avalonia.Markup.Xaml;
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

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
