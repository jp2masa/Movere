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
    }
}
