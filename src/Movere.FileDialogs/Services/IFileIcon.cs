using System;
using System.IO;

namespace Movere.Services
{
    public interface IFileIcon : IDisposable
    {
        void Save(Stream stream);
    }
}
