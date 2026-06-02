namespace Movere.Services
{
    public interface IDialogView<in TResult>
    {
        IDialogHost Host { get; }

        void Close(TResult result);
    }
}
