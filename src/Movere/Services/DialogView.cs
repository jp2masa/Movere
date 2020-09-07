using Avalonia.Controls;

namespace Movere.Services
{
    public sealed class DialogView<TResult> : IDialogView<TResult>
    {
        private readonly Window _view;

        public DialogView(Window view)
        {
            _view = view;
        }

        public void Close(TResult result) => _view.Close(result);
    }
}
