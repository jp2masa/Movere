using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Platform;
using Avalonia.Platform;
using Avalonia.Platform.Storage;

using Movere.Avalonia.Services;

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
                    _options.PreferWindowDialogs && topLevel is Window window
                        ? () => new WindowDialogHost(GetApplication(), window)
                        : () => new OverlayDialogHost(GetApplication(), topLevel),
                    _options
                );

        internal static Application GetApplication() =>
            Application.Current
                ?? throw new InvalidOperationException(
                    $"{nameof(Application)}.{nameof(Application.Current)} is null!"
                );
    }
}
