using System;

namespace Movere.Models
{
    public sealed record MessageDialogOptions
    {
        public MessageDialogOptions(
            LocalizedString message,
            LocalizedString title
        )
        {
            Message = message;
            Title = title;
        }

        [Obsolete("Use `new MessageDialogOptions(message, title)` instead")]
        public MessageDialogOptions(
            string message,
            string title = "Message",
            IDialogIcon? icon = null,
            DialogResultSet? dialogResults = null)
        {
            Message = message;
            Title = title;

            Icon = icon ?? DialogIcon.None;

            DialogResults = dialogResults ?? DialogResultSet.OK;
        }

        public LocalizedString Message { get; init; }

        public LocalizedString Title { get; init; }

        public IDialogIcon Icon { get; init; } =
            DialogIcon.None;

        public DialogResultSet DialogResults { get; init; } =
            DialogResultSet.OK;
    }
}
