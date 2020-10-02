using Avalonia.Media.Imaging;

namespace Movere.Services
{
    public interface IFileIconProvider
    {
        IBitmap? GetFileIcon(string filePath);
    }
}
