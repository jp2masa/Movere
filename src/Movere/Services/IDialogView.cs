namespace Movere.Services
{
    public interface IDialogView<TResult>
    {
        void Close(TResult result);
    }
}
