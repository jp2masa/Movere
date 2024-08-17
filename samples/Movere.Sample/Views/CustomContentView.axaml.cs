using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Movere.Sample.ViewModels;

namespace Movere.Sample.Views
{
    internal sealed partial class CustomContentView : ReactiveUserControl<CustomContentViewModel>
    {
        public CustomContentView()
        {
            InitializeComponent();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
