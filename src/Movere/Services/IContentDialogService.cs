using System.Threading.Tasks;

using Movere.Models;

namespace Movere.Services
{
    public interface IContentDialogService<TContent>
    {
        Task<DialogResult> ShowDialogAsync(ContentDialogOptions<TContent> options);
    }
}
