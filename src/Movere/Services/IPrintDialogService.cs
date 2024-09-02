using System.Threading.Tasks;

using Movere.Models;

namespace Movere.Services
{
    public interface IPrintDialogService
    {
        Task<bool> ShowDialogAsync(PrintDialogOptions options);
    }
}
