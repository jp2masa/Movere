using System.Threading.Tasks;

using Movere.Models;

namespace Movere.Services
{
    public interface ISaveFileDialogService
    {
        Task<SaveFileDialogResult> ShowDialogAsync();
    }
}
