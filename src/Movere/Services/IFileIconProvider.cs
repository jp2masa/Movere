namespace Movere.Services
{
    public interface IFileIconProvider
    {
        IFileIcon? GetFileIcon(string filePath);
    }
}
