using System;

using Movere.Models;

namespace Movere.ViewModels
{
    internal sealed class MessageDialogViewModel
    {
        private readonly IDialogIcon _icon;
        private readonly Lazy<IBitmap?> _bitmap;

        public MessageDialogViewModel(IDialogIcon icon, LocalizedString message)
        {
            _icon = icon;
            _bitmap = new Lazy<IBitmap?>(() => _icon.LoadIcon());

            Message = message;
        }

        public IBitmap? Icon =>
            _bitmap.Value;

        public LocalizedString Message { get; }
    }
}
