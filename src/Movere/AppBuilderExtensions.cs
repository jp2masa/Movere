using System;
using System.Collections.Immutable;

using Avalonia;
using Avalonia.Controls.Platform;

using Movere.Storage;

namespace Movere
{
    public static class AppBuilderExtensions
    {
        internal static readonly AttachedProperty<ImmutableArray<Extension>> MovereExtensionsProperty =
            AvaloniaProperty.RegisterAttached<Application, ImmutableArray<Extension>>(
                "MovereExtensions",
                typeof(AppBuilderExtensions),
                ImmutableArray<Extension>.Empty
            );

        internal sealed record Extension(Type Type, Func<object> Factory);

        internal static void AddExtension(this Application application, Extension extension) =>
            application.SetValue(
                MovereExtensionsProperty,
                application
                    .GetValue(MovereExtensionsProperty)
                    .Add(extension)
            );

        public static AppBuilder UseMovereStorageProvider(
            this AppBuilder builder,
            MovereStorageProviderOptions? options = null
        ) =>
            builder.AfterSetup(
                _ =>
                    AvaloniaLocator.CurrentMutable
                        .Bind<IStorageProviderFactory>()
                        .ToConstant(new MovereStorageProviderFactory(options))
            );

        [Obsolete]
        public static AppBuilder UseMovereSystemDialogs(this AppBuilder builder) =>
            builder.AfterSetup(
                _ =>
                    AvaloniaLocator.CurrentMutable
                        .Bind<ISystemDialogImpl>()
                        .ToSingleton<MovereSystemDialogImpl>()
            );

        [Obsolete]
        public static AppBuilder UseMovere(this AppBuilder builder) =>
            builder
                .UseMovereSystemDialogs()
                .UseMovereStorageProvider();
    }
}
