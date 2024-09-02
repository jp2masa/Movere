using Avalonia.ReactiveUI;

using Movere.ViewModels;

namespace Movere.Views
{
    internal sealed partial class OpenFileDialog : ReactiveUserControl<OpenFileDialogViewModel>
    {
        public OpenFileDialog()
        {
            InitializeComponent();
        }
    }
}
