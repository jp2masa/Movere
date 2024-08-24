using Movere.Models;

namespace Movere.ViewModels
{
    internal interface IDialogWindowViewModel
    {
        LocalizedString Title { get; }

        object Content { get; }

        bool IsBusy { get; }

        bool OnClosing();
    }
}
