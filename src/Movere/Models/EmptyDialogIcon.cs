namespace Movere.Models
{
    internal class EmptyDialogIcon : IDialogIcon
    {
        public static EmptyDialogIcon Instance { get; } = new EmptyDialogIcon();

        public IBitmap? LoadIcon() =>
            null;
    }
}
