using System.IO;

namespace Movere.Models
{
    public class AddressPiece
    {
        public AddressPiece(string name, DirectoryInfo directory)
        {
            Name = name;
            Directory = directory;
        }

        public string Name { get; }

        public DirectoryInfo Directory { get; }
    }
}
