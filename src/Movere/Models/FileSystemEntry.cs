namespace Movere.Models
{
    public abstract class FileSystemEntry
    {
        public abstract string Name { get; }

        public abstract string FullPath { get; }
    }
}
