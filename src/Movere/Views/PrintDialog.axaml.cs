using Avalonia.ReactiveUI;

using Movere.ViewModels;

namespace Movere.Views
{
    internal sealed partial class PrintDialog : ReactiveUserControl<PrintDialogViewModel>
    {
        public PrintDialog()
        {
            InitializeComponent();
        }
    }
}
