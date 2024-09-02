using Movere.Models;

namespace Movere.ViewModels
{
    internal sealed class DesignDialogWindowViewModel : IDialogWindowViewModel
    {
        public LocalizedString Title =>
            new LocalizedString("Design Dialog");

        public object Content { get; set; } =
            "Dialog Content";

        public bool IsBusy =>
            false;

        public bool OnClosing() =>
            false;
    }
}
