namespace Movere.Models
{
    public sealed class MessageDialogOptions
    {
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

        public string Message { get; }

        public string Title { get; }

        public IDialogIcon Icon { get; }

        public DialogResultSet DialogResults { get; }
    }
}
