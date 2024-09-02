using System;

using Avalonia.Controls;
using Avalonia.Controls.Platform;
using Avalonia.Platform;
using Avalonia.Platform.Storage;

namespace Movere.Storage
{
    internal sealed class MovereStorageProviderFactory(
        MovereStorageProviderOptions? options = null
    )
        : IStorageProviderFactory
    {
        private readonly MovereStorageProviderOptions _options =
            options ?? MovereStorageProviderOptions.Default;

        public IStorageProvider CreateProvider(TopLevel topLevel) =>
            _options.IsFallback
            && (
                topLevel.PlatformImpl?
                    .TryGetFeature<IStorageProvider>(out var provider)
                ?? false
            )
                ? provider
                : new MovereStorageProvider(
                    (topLevel as Window)
                        ?? throw new NotSupportedException("Movere is only supported on Window top levels!")
                );
    }
}
