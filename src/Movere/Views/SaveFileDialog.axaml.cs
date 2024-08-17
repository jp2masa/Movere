#if AVALONIA_DIAGNOSTICS
using Avalonia;
#endif
using Avalonia.ReactiveUI;

using Movere.ViewModels;

namespace Movere.Views
{
    internal sealed partial class SaveFileDialog : ReactiveWindow<SaveFileDialogViewModel>
    {
        public SaveFileDialog()
        {
            InitializeComponent();
#if AVALONIA_DIAGNOSTICS
            this.AttachDevTools();
#endif
        }
    }
}
