using System;

using Avalonia.Controls;
using Avalonia.Controls.Platform;
using Avalonia.Platform.Storage;

namespace Movere.Storage
{
    internal sealed class MovereStorageProviderFactory : IStorageProviderFactory
    {
        public IStorageProvider CreateProvider(TopLevel topLevel) =>
            new MovereStorageProvider(
                (topLevel as Window)
                    ?? throw new NotSupportedException("Movere is only supported on Window top levels!")
            );
    }
}
