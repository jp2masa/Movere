using Avalonia.Markup.Xaml;

using Movere.Controls;

namespace Movere.Views
{
    public sealed class ListFolderView : ItemsControlView
    {
        public ListFolderView()
        {
            InitializeComponent();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
