namespace Movere.Models
{
    internal sealed class EmptyDialogIcon : IDialogIcon
    {
        public static EmptyDialogIcon Instance { get; } = new EmptyDialogIcon();

        private EmptyDialogIcon()
        {
        }

        public IBitmap? LoadIcon() =>
            null;
    }
}
