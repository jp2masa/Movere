using System.Drawing.Printing;
using System.Threading.Tasks;

namespace Movere.Services
{
    public interface IPrintDialogService
    {
        Task<bool> ShowDialogAsync(PrintDocument document);
    }
}