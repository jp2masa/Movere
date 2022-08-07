using System.Threading.Tasks;

namespace Movere.Services
{
    public interface IContentDialogService<TContent, TResult>
    {
        Task<TResult> ShowDialogAsync(ContentDialogOptions<TContent, TResult> options);
    }
}
