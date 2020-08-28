using System.Threading.Tasks;

using Movere.Models;

namespace Movere.Services
{
    public interface IOpenFileDialogService
    {
        Task<OpenFileDialogResult> ShowDialogAsync(bool allowMultipleSelection = false);
    }
}
