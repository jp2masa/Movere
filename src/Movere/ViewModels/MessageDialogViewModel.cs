using Avalonia.Media.Imaging;

using Movere.Models;

namespace Movere.ViewModels
{
    internal sealed class MessageDialogViewModel
    {
        private readonly IDialogIcon _icon;

        public MessageDialogViewModel(IDialogIcon icon, string message)
        {
            _icon = icon;

            Message = message;
        }

        public IBitmap? Icon => _icon.LoadIcon();

        public string Message { get; }
    }
}
