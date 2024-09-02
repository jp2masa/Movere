using System;

using Avalonia;
using Avalonia.ReactiveUI;

using Movere.ViewModels;

namespace Movere.Views
{
    internal sealed partial class DialogOverlay : ReactiveUserControl<IDialogWindowViewModel>
    {
        static DialogOverlay()
        {
            ClipToBoundsProperty.OverrideDefaultValue<DialogOverlay>(false);
        }

        public DialogOverlay()
        {
            InitializeComponent();
        }

        public bool OnClosing() =>
            ViewModel?.OnClosing() ?? true;

        protected override Size MeasureOverride(Size availableSize)
        {
            var desiredSize = base.MeasureOverride(availableSize);

            return new Size(
                Math.Max(availableSize.Width, desiredSize.Width),
                Math.Max(availableSize.Height, desiredSize.Height)
            );
        }
    }
}
