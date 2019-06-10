using System.Collections.Generic;

using Avalonia.Media.Imaging;

namespace Movere.Models
{
    public sealed class MessageDialogOptions
    {
        public MessageDialogOptions(IDialogIcon icon, string title, string message, IReadOnlyList<IDialogResult> dialogResults)
        {
            Icon = icon;

            Title = title;
            Message = message;

            DialogResults = dialogResults;
        }

        public IDialogIcon Icon { get; }

        public string Title { get; }

        public string Message { get; }

        public IReadOnlyList<IDialogResult> DialogResults { get; }
    }
}
