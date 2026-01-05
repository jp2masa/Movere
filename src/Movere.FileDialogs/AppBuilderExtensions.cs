using System;

using Autofac;

using Avalonia;
using Avalonia.Controls.Platform;

using Movere.Storage;

namespace Movere.FileDialogs
{
    public static partial class AppBuilderExtensions
    {
        public static AppBuilder UseMovereStorageProvider(
            this AppBuilder builder,
            MovereStorageProviderOptions? options = null
        ) =>
            builder.AfterSetup(
                x =>
                {
                    x.Instance?.AddExtension(
                        new Movere.AppBuilderExtensions.Extension(
                            x => x.RegisterAssemblyModules(typeof(AppBuilderExtensions).Assembly)
                        )
                    );

                    AvaloniaLocator.CurrentMutable
                        .Bind<IStorageProviderFactory>()
                        .ToConstant(new MovereStorageProviderFactory(options));
                }
            );

        [Obsolete]
        public static AppBuilder UseMovereSystemDialogs(this AppBuilder builder) =>
            builder.AfterSetup(
                x =>
                {
                    x.Instance?.AddExtension(
                        new Movere.AppBuilderExtensions.Extension(
                            x => x.RegisterAssemblyModules(typeof(AppBuilderExtensions).Assembly)
                        )
                    );

                    AvaloniaLocator.CurrentMutable
                        .Bind<ISystemDialogImpl>()
                        .ToSingleton<MovereSystemDialogImpl>();
                }
            );

        [Obsolete]
        public static AppBuilder UseMovere(this AppBuilder builder) =>
            builder
                .UseMovereSystemDialogs()
                .UseMovereStorageProvider();
    }
}
