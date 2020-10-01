using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Movere.Sample.ViewModels;

namespace Movere.Sample.Views
{
    internal sealed class FieldView : ReactiveUserControl<FieldViewModel>
    {
        public FieldView()
        {
            InitializeComponent();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
