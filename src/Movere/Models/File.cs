using System;
using System.IO;
using System.Threading.Tasks;

namespace Movere.Models
{
    public sealed class File : FileSystemEntry, IEquatable<File>
    {
        private readonly FileInfo _info;

        public File(FileInfo info)
        {
            _info = info;
        }

        public override string Name => _info.Name;

        public override string FullPath => _info.FullName;

        public override Task DeleteAsync()
        {
            _info.Delete();
            return Task.CompletedTask;
        }

        public bool Equals(File other) => other != null && String.Equals(FullPath, other.FullPath, StringComparison.Ordinal);

        public override bool Equals(object obj) => obj is File file && Equals(file);

        public override int GetHashCode() => HashCode.Combine(FullPath);

        public override string ToString() => $"File: '{FullPath}'";
    }
}
