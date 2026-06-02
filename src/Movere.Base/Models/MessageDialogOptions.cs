using System;

namespace Movere.Models
{
    public sealed record MessageDialogOptions(
        LocalizedString Message,
        LocalizedString Title
    )
    {
        [Obsolete("Use `new MessageDialogOptions(message, title)` instead")]
        public MessageDialogOptions(
            string message,
            string title = "Message",
            IDialogIcon? icon = null,
            DialogResultSet? dialogResults = null
        )
            : this((LocalizedString)message, title)
        {
            Icon = icon ?? DialogIcon.None;

            DialogResults = dialogResults ?? DialogResultSet.OK;
        }

        public IDialogIcon Icon { get; init; } =
            DialogIcon.None;

        public DialogResultSet DialogResults { get; init; } =
            DialogResultSet.OK;
    }
}
