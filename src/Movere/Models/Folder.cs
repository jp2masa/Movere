using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;

namespace Movere.Models
{
    public sealed class Folder : FileSystemEntry, IEquatable<Folder>
    {
        private readonly DirectoryInfo _info;

        public Folder(DirectoryInfo info)
        {
            _info = info;
        }

        public override string Name => _info.Name;

        public override string FullPath => _info.FullName;

        public Folder? Parent => _info.Parent == null ? null : new Folder(_info.Parent);

        public IEnumerable<Folder> Folders => GetFolders();

        public IEnumerable<File> Files => GetFiles();

        public IEnumerable<FileSystemEntry> Entries => Folders.Concat<FileSystemEntry>(Files);

        public bool Equals(Folder other) => other != null && String.Equals(FullPath, other.FullPath, StringComparison.Ordinal);

        public override bool Equals(object obj) => obj is Folder folder && Equals(folder);

        public override int GetHashCode() => HashCode.Combine(FullPath);

        public override string ToString() => $"Folder: '{FullPath}'";

        private IEnumerable<Folder> GetFolders()
        {
            try
            {
                return _info.EnumerateDirectories().Select(NewFolder);
            }
            catch (Exception e) when (e is SecurityException || e is UnauthorizedAccessException)
            {
                return Array.Empty<Folder>();
            }
        }

        private IEnumerable<File> GetFiles()
        {
            try
            {
                return _info.EnumerateFiles().Select(NewFile);
            }
            catch (Exception e) when (e is SecurityException || e is UnauthorizedAccessException)
            {
                return Array.Empty<File>();
            }
        }

        private static Folder NewFolder(DirectoryInfo folder) => new Folder(folder);

        private static File NewFile(FileInfo file) => new File(file);
    }
}
