#if AVALONIA_DIAGNOSTICS
using Avalonia;
#endif
using Avalonia.Controls;
using Avalonia.ReactiveUI;

using Movere.ViewModels;

namespace Movere.Views
{
    internal sealed partial class DialogWindow : ReactiveWindow<IDialogWindowViewModel>
    {
        public DialogWindow()
        {
            InitializeComponent();
#if AVALONIA_DIAGNOSTICS
            this.AttachDevTools();
#endif
        }

        protected override void OnClosing(WindowClosingEventArgs e)
        {
            if (ViewModel is { } vm)
            {
                e.Cancel = !vm.OnClosing();
            }
        }
    }
}
