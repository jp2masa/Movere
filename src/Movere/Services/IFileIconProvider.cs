namespace Movere.Services
{
    internal interface IFileIconProvider
    {
        IFileIcon? GetFileIcon(string filePath);
    }
}
