using System;

using Autofac;

using Avalonia;
using Avalonia.Controls.Platform;

using Movere.Storage;

namespace Movere.FileDialogs
{
    public static class AppBuilderExtensions
    {
        private static readonly Movere.AppBuilderExtensions.Extension s_extension =
            new Movere.AppBuilderExtensions.Extension(x => x.RegisterAssemblyModules(typeof(AppBuilderExtensions).Assembly));

        public static AppBuilder UseMovereFileDialogs(this AppBuilder builder) =>
            builder.AfterSetup(x => x.Instance?.AddExtension(s_extension));

        public static AppBuilder UseMovereStorageProvider(
            this AppBuilder builder,
            MovereStorageProviderOptions? options = null
        ) =>
            builder
                .UseMovereFileDialogs()
                .AfterSetup(
                    _ =>
                        AvaloniaLocator.CurrentMutable
                            .Bind<IStorageProviderFactory>()
                            .ToConstant(new MovereStorageProviderFactory(options))
                );

        [Obsolete("Replace with UseMovereStorageProvider.")]
        public static AppBuilder UseMovere(this AppBuilder builder) =>
            builder
                .UseMovereStorageProvider();
    }
}
