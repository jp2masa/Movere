using Movere.Models;

namespace Movere.ViewModels
{
    public interface IDialogWindowViewModel
    {
        LocalizedString Title { get; }

        object Content { get; }

        bool IsBusy { get; }

        bool OnClosing();
    }

    public interface IDialogWindowViewModel<out TContent> : IDialogWindowViewModel
    {
        new TContent Content { get; }
    }
}
