#if AVALONIA_DIAGNOSTICS
using Avalonia;
#endif
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Movere.Sample.ViewModels;

namespace Movere.Sample.Views
{
    internal sealed class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
#if AVALONIA_DIAGNOSTICS
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
