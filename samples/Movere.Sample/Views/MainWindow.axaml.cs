#if AVALONIA_DIAGNOSTICS
using Avalonia;
#endif
using Avalonia.ReactiveUI;

using Movere.Sample.ViewModels;

namespace Movere.Sample.Views
{
    internal sealed partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
#if AVALONIA_DIAGNOSTICS
            this.AttachDevTools();
#endif
        }
    }
}
