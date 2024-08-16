using System;

using Avalonia.Media.Imaging;

using Movere.Models;

namespace Movere.ViewModels
{
    internal sealed class MessageDialogViewModel
    {
        private readonly IDialogIcon _icon;
        private readonly Lazy<Bitmap?> _bitmap;

        public MessageDialogViewModel(IDialogIcon icon, string message)
        {
            _icon = icon;
            _bitmap = new Lazy<Bitmap?>(LoadIcon);

            Message = message;
        }

        public Bitmap? Icon =>
            _bitmap.Value;

        public string Message { get; }

        private Bitmap? LoadIcon()
        {
            if (_icon.LoadIcon() is { } icon)
            {
                using var stream = icon.Open();
                return new Bitmap(stream);
            }

            return null;
        }
    }
}
