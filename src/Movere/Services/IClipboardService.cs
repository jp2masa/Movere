using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movere.Services
{
    public interface IClipboardService
    {
        Task ClearAsync();

        Task<string> GetTextAsync();

        Task SetTextAsync(string text);

        Task<IReadOnlyCollection<string>> GetFilesAsync();

        Task SetFilesAsync(IReadOnlyCollection<string> files);
    }
}
