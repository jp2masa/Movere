using System.ComponentModel;

#if AVALONIA_DIAGNOSTICS
using Avalonia;
#endif
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Movere.ViewModels;

namespace Movere.Views
{
    internal sealed class ContentDialog : ReactiveWindow<IContentDialogViewModel>
    {
        public ContentDialog()
        {
            InitializeComponent();
#if AVALONIA_DIAGNOSTICS
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        protected override void OnClosing(CancelEventArgs e)
        {
            if (ViewModel is IContentDialogViewModel vm)
            {
                e.Cancel = !vm.OnClosing();
            }
        }
    }
}
