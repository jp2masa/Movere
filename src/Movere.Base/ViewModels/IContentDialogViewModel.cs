using System.Collections;
using System.Windows.Input;

namespace Movere.ViewModels
{
    internal interface IContentDialogViewModel
    {
        object? Content { get; }

        IEnumerable Actions { get; }

        ICommand CloseCommand { get; }
    }
}
