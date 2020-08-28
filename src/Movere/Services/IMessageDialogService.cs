using System.Threading.Tasks;

using Movere.Models;

namespace Movere.Services
{
    public interface IMessageDialogService
    {
        Task<IDialogResult?> ShowMessageDialogAsync(MessageDialogOptions options);
    }
}
