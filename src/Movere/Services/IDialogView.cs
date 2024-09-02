namespace Movere.Services
{
    public interface IDialogView<in TResult>
    {
        void Close(TResult result);
    }
}
