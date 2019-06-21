using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;

namespace Movere.Models
{
    public sealed class Folder : IEquatable<Folder>
    {
        private readonly DirectoryInfo _info;

        public Folder(DirectoryInfo info)
        {
            _info = info;
        }

        public string Name => _info.Name;

        public string FullPath => _info.FullName;

        public Folder? Parent => _info.Parent == null ? null : new Folder(_info.Parent);

        public IEnumerable<Folder> Folders => GetFolders();

        public IEnumerable<FileSystemInfo> EnumerateEntries()
        {
            try
            {
                return _info.EnumerateFileSystemInfos();
            }
            catch (Exception e) when (e is SecurityException || e is UnauthorizedAccessException)
            {
                return Array.Empty<FileSystemInfo>();
            }
        }

        public bool Equals(Folder other) => String.Equals(FullPath, other.FullPath, StringComparison.Ordinal);

        public override bool Equals(object obj) => obj is Folder file && Equals(file);

        public override int GetHashCode() => HashCode.Combine(FullPath);

        public override string ToString() => $"Folder: '{FullPath}'";

        private IEnumerable<Folder> GetFolders()
        {
            try
            {
                return _info.EnumerateDirectories().Select(d => new Folder(d));
            }
            catch (Exception e) when (e is SecurityException || e is UnauthorizedAccessException)
            {
                return Array.Empty<Folder>();
            }
        }
    }
}
